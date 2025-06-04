using eUseControl.BusinessLogic.BusinessLayer;
using eUseControl.BusinessLogic.Interfaces;

namespace eUseControl.BusinessLogic
{
    public class BusinessLogicManager
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();
        }

        public IProduct GetProductBL()
        {
            return new ProductBL();
        }

        public IProfile GetProfileBL()
        {
            return new ProfileBL();
        }

        public IReview GetReviewBL()
        {
            return new ReviewBL();
        }

        public IWishlist GetWishlistBL()
        {
            return new WishlistBL();
        }

        public ICart GetCartBL()
        {
            return new CartBL();
        }

        public IOrder GetOrderBL()
        {
            return new OrderBL();
        }

        public ITransaction GetTransactionBL()
        {
            return new TransactionBL();
        }
    }
}
