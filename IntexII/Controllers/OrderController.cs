using Microsoft.AspNetCore.Mvc;
using IntexII.Models;
using IntexII.Models.ViewModels;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace IntexII.Controllers
{
    public class OrderController : Controller
    {
        private IIntexRepository _repo;
        private Cart cart;
        private readonly InferenceSession _session;
        private readonly IWebHostEnvironment _environment;

        public OrderController(IIntexRepository temp, Cart cartService, IWebHostEnvironment environment)
        {
            _repo = temp;
            cart = cartService;
            _environment = environment;

            // Combine the base path with the relative path to your ONNX file
            string modelPath = Path.Combine(_environment.ContentRootPath, "decision_tree_model.onnx");
            _session = new InferenceSession(modelPath);

        }

        [Authorize(Roles = "User")]
        public IActionResult Checkout()
        {
            var checkoutDetails = new CheckoutViewModel
            {
                Cart = cart
            };
            
            return View(checkoutDetails);

        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel checkoutDetails)
        {



            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                //Calculate days since Jan 1 2022
                //var january12022 = new DateTime(2022, 1, 1);
                //var daysSinceJan2022 = Math.Abs((checkoutDetails.Order.OrderDate - january12022).Days);



                var classTypeDict = new Dictionary<int, string>
                {
                    { 0, "Not Fraud" },
                    { 1, "Fraud" }
                };

                checkoutDetails.Order.Amount = cart.CalculateTotal();


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

                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };
                string predictionResult = null;
                //long predictionResultInt;

                using (var results = _session.Run(inputs))
                {
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
                        .ToArray();
                    predictionResult = prediction != null && prediction.Length > 0 ? classTypeDict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
                    
                }

                checkoutDetails.Order.Fraud = 0; //Default to 0, can be changed upon order review
                checkoutDetails.Order.FraudFlag = predictionResult;


                
                _repo.AddOrder(checkoutDetails.Order);

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
