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

        public PaymentController()
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
            _transaction = bl.GetTransactionBL();
            _cart = bl.GetCartBL();
            _product = bl.GetProductBL();
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
        public ActionResult ProcessTransaction(TransactionCompact transactionCompact)
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

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TransactionCompact, TransactionData>();
                });

                var mapper = config.CreateMapper();
                var transaction = mapper.Map<TransactionData>(transactionCompact);

                var result = _transaction.ProcessPayment(transaction, user.Id);

                if (result.Status)
                {
                    var allCartItems = _cart.GetCartItemsByUserId(user.Id);

                    var updateResult = _product.UpdateProductQuantitiesAfterOrder(allCartItems);
                    if (!updateResult.Status)
                    {
                        return RedirectToAction("Payment", "Payment", new { error = true, orderId = transactionCompact.OrderId });
                    }

                    var clearResult = _cart.ClearCartItemsAfterOrder(user.Id);
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