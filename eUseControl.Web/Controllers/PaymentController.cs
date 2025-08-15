using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Payment;
using eUseControl.Web.Models.Payment;

namespace eUseControl.Web.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;
        private readonly ICart _cart;
        private readonly IProduct _product;
        private readonly IMapper _mapper;

        public PaymentController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
            _transaction = bl.GetTransactionBL();
            _cart = bl.GetCartBL();
            _product = bl.GetProductBL();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Payment(int orderId)
        {
            var model = new TransactionCompact
            {
                OrderId = orderId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcessTransaction(TransactionCompact transactionCompact)
        {
            if (ModelState.IsValid)
            {
                var cookie = Request.Cookies["X-KEY"]?.Value;
                if (string.IsNullOrEmpty(cookie))
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var user = _session.GetUserByCookie(cookie);
                if (user == null)
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var transaction = _mapper.Map<TransactionData>(transactionCompact);

                var result = await _transaction.ProcessPayment(transaction, user.Id);

                if (result.Status)
                {
                    var allCartItems = await _cart.GetCartItemsByUserId(user.Id);

                    var updateResult = await _product.UpdateProductQuantitiesAfterOrder(allCartItems);
                    if (!updateResult.Status)
                    {
                        return RedirectToAction("Payment", "Payment", new { error = true, orderId = transactionCompact.OrderId });
                    }

                    var clearResult = await _cart.ClearCartItemsAfterOrder(user.Id);
                    if (!clearResult.Status)
                    {
                        return RedirectToAction("Payment", "Payment", new { error = true, orderId = transactionCompact.OrderId });
                    }

                    return RedirectToAction("OrderConfirmation", "Order", new { success = true, orderId = transactionCompact.OrderId });
                }
                else
                {
                    return RedirectToAction("OrderFailure", "Order", new { error = true, orderId = transactionCompact.OrderId });
                }
            }
            else
            {
                return RedirectToAction("Payment", "Payment", new { error = true, orderId = transactionCompact.OrderId });
            }
        }
    }
}