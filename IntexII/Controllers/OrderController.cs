using Microsoft.AspNetCore.Mvc;
using IntexII.Models;
using IntexII.Models.ViewModels;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.AspNetCore.Identity;

namespace IntexII.Controllers
{
    public class OrderController : Controller
    {
        //Create variable to store database info, identity info, and get variables for classification model
        private IIntexRepository _repo;
        private Cart cart;
        private readonly InferenceSession _session;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IIntexRepository temp, Cart cartService, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _repo = temp;
            cart = cartService;
            _environment = environment;
            _userManager = userManager;

            string modelPath;

            if (_environment.IsDevelopment())
            {
                // Development environment
                modelPath = Path.Combine(_environment.ContentRootPath, "decision_tree_model.onnx");
            }
            else
            {
                // Production environment (deployed to Azure)              
                modelPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "decision_tree_model.onnx");

            }

            _session = new InferenceSession(modelPath);

        }

        [Authorize(Roles = "User")] //Only users can actually checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            //Create a new checkoutviewmodel and set the cart model in it to the current cart
            var checkoutDetails = new CheckoutViewModel
            {
                Cart = cart
            };
            
            return View(checkoutDetails);

        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(CheckoutViewModel checkoutDetails)
        {



            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                
                //Set up dictionary for values returned from fraud classification model. Calculate the total of the cart and set it for order. 
                //Get the user information and use it to set customer id. If no customer assigned, default it to 1.
                var classTypeDict = new Dictionary<int, string>
                {
                    { 0, "Not Fraud" },
                    { 1, "Fraud" }
                };

                checkoutDetails.Order.Amount = cart.CalculateTotal();

                ApplicationUser user = await _userManager.GetUserAsync(User);
                int? customerId = user.CustomerId;
                if (customerId.HasValue)
                {
                    checkoutDetails.Order.CustomerId = user.CustomerId.Value;
                }
                else
                {
                    checkoutDetails.Order.CustomerId= 1;
                }
                

                //Get inputs for model by dummy coding. 
                var input = new List<float>
                    {
                        
                        checkoutDetails.Order.CustomerId,
                        checkoutDetails.Order.Time,
				        //fix amount if it’s null
				        (float)(checkoutDetails.Order.Amount),

				        //fix date
				        //daysSinceJan2022,

				        //check the dummy coded data
				        checkoutDetails.Order.DayOfWeek == "Mon" ? 1 : 0,
                        checkoutDetails.Order.DayOfWeek == "Sat" ? 1 : 0,
                        checkoutDetails.Order.DayOfWeek == "Sun" ? 1 : 0,
                        checkoutDetails.Order.DayOfWeek == "Thu" ? 1 : 0,
                        checkoutDetails.Order.DayOfWeek == "Tue" ? 1 : 0,
                        checkoutDetails.Order.DayOfWeek == "Wed" ? 1 : 0,

                        checkoutDetails.Order.EntryMode == "Pin" ? 1 : 0,
                        checkoutDetails.Order.EntryMode == "Tap" ? 1 : 0,

                        checkoutDetails.Order.TypeOfTransaction == "Online" ? 1 : 0,
                        checkoutDetails.Order.TypeOfTransaction == "POS" ? 1 : 0,

                        checkoutDetails.Order.CountryOfTransaction == "India" ? 1 : 0,
                        checkoutDetails.Order.CountryOfTransaction == "Russia" ? 1 : 0,
                        checkoutDetails.Order.CountryOfTransaction == "USA" ? 1 : 0,
                        checkoutDetails.Order.CountryOfTransaction == "UnitedKingdom" ? 1 : 0,

				        //use countryoftransaction if shipping address is null
				        (checkoutDetails.Order.ShippingAddress ?? checkoutDetails.Order.CountryOfTransaction) == "India" ? 1 : 0,
                        (checkoutDetails.Order.ShippingAddress ?? checkoutDetails.Order.CountryOfTransaction) == "Russia" ? 1 : 0,
                        (checkoutDetails.Order.ShippingAddress ?? checkoutDetails.Order.CountryOfTransaction) == "USA" ? 1 : 0,
                        (checkoutDetails.Order.ShippingAddress ?? checkoutDetails.Order.CountryOfTransaction) == "UnitedKingdom" ? 1 : 0,

                        checkoutDetails.Order.Bank == "HSBC" ? 1 : 0,
                        checkoutDetails.Order.Bank == "Halifax" ? 1 : 0,
                        checkoutDetails.Order.Bank == "Lloyds" ? 1 : 0,
                        checkoutDetails.Order.Bank == "Metro" ? 1 : 0,
                        checkoutDetails.Order.Bank == "Monzo" ? 1 : 0,
                        checkoutDetails.Order.Bank == "RBS" ? 1 : 0,

                        checkoutDetails.Order.TypeOfCard == "Visa" ? 1 : 0


                    };

                //Run model and return prediction of fraud or not fraud. Set new order FraudFlag column to that. 
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };
                string predictionResult = null;

                using (var results = _session.Run(inputs))
                {
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
                        .ToArray();
                    predictionResult = prediction != null && prediction.Length > 0 ? classTypeDict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
                    
                }

                checkoutDetails.Order.Fraud = 0; //Default to 0, can be changed upon order review
                checkoutDetails.Order.FraudFlag = predictionResult;


                //Save the order to the database
                _repo.AddOrder(checkoutDetails.Order);

                //save a lineitem to the database for each item in the cart
                foreach (var l in cart?.Lines ?? Enumerable.Empty<Cart.CartLine>())
                {
                    var lineItem = new LineItem
                    {
                        TransactionId = checkoutDetails.Order.TransactionId,
                        ProductId = l.Product.ProductId,
                        Quantity = l.Quantity,

                    };

                    _repo.AddLineItem(lineItem);
                }



                cart.Clear();

                return View("OrderConfirmation", checkoutDetails.Order);      
            }
            else
            {
                return View();
            }
        }
    }


}
