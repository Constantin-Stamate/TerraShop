namespace eUseControl.BusinessLogic.Migrations.Product
{
    using System;
    using System.Data.Entity.Migrations;
    using eUseControl.Domain.Entities.Product;
    using eUseControl.Domain.Enums;

    internal sealed class Configuration : DbMigrationsConfiguration<eUseControl.BusinessLogic.DBModel.ProductContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Product";
        }

        protected override void Seed(eUseControl.BusinessLogic.DBModel.ProductContext context)
        {
            context.Products.AddOrUpdate(p => p.ProductName,

                new ProductDbTable
                {
                    Id = 1,
                    UserId = 1,
                    CategoryId = 1,
                    ProductName = "Nourishing Face Cream",
                    ProductAddress = "Nature Street 10, Bucharest",
                    ProductQuantity = 50,
                    ProductQuality = "Premium",
                    ProductPrice = 29.99m,
                    ProductRegion = "Romania",
                    ProductImageUrl = "/Uploads/products/item-1.jpg",
                    ProductDescription = "Indulge your skin with our Nourishing Face Cream, formulated with a rich blend of natural oils, vitamins, and botanical extracts. Designed to hydrate deeply and restore the skin’s natural elasticity, this cream is perfect for daily use on all skin types. Free from parabens and synthetic fragrances, it absorbs quickly, leaving your face soft, radiant, and revitalized. A luxurious, eco-friendly choice for conscious skincare enthusiasts.",
                    ProductPostDate = DateTime.Now,
                    ProductStatus = ProductStatus.Available,
                    RecommendationStatus = RecommendationStatus.Preferred,
                    ProductRating = 5
                },

               new ProductDbTable
               {
                   Id = 2,
                   UserId = 1,
                   CategoryId = 1,
                   ProductName = "Bamboo Toothbrush",
                   ProductAddress = "Green Avenue 42, Cluj-Napoca",
                   ProductQuantity = 100,
                   ProductQuality = "Premium",
                   ProductPrice = 12.50m,
                   ProductRegion = "Romania",
                   ProductImageUrl = "/Uploads/products/item-2.jpg",
                   ProductDescription = "Make a sustainable switch with our Bamboo Toothbrush, crafted from biodegradable bamboo and soft, BPA-free bristles. Gentle on your gums and tough on plaque, it’s the perfect eco-conscious alternative to plastic brushes. Designed for comfort and effectiveness, this toothbrush helps you maintain oral hygiene while caring for the planet. Ideal for all ages, it's a simple yet impactful step towards zero-waste living.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 3,
                   UserId = 1,
                   CategoryId = 2,
                   ProductName = "Eco-Friendly Laundry Detergent",
                   ProductAddress = "Eco Street 15, Timisoara",
                   ProductQuantity = 75,
                   ProductQuality = "High",
                   ProductPrice = 19.99m,
                   ProductRegion = "Romania",
                   ProductImageUrl = "/Uploads/products/item-3.jpg",
                   ProductDescription = "Clean your clothes with care for the planet using our Eco-Friendly Laundry Detergent. Made with natural, biodegradable ingredients, it effectively removes stains and odors without the harsh chemicals found in conventional detergents. Gentle on your skin and safe for sensitive fabrics, this detergent is perfect for households looking to minimize their environmental impact. Free from synthetic fragrances, it's a perfect choice for conscious consumers.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 4,
                   UserId = 1,
                   CategoryId = 2,
                   ProductName = "Biodegradable Trash Bags",
                   ProductAddress = "101 Maple St, Toronto",
                   ProductQuantity = 100,
                   ProductQuality = "Premium",
                   ProductPrice = 14.99m,
                   ProductRegion = "Canada",
                   ProductImageUrl = "/Uploads/products/item-4.jpg",
                   ProductDescription = "Make an eco-friendly choice with our Biodegradable Trash Bags. Made from plant-based materials, these bags break down naturally over time, reducing plastic waste and promoting sustainability. Strong and reliable, they are perfect for daily use in homes, offices, and businesses. These trash bags are designed to handle all types of waste, while helping you contribute to a cleaner planet. Free from harmful chemicals, they offer a safe and responsible solution for waste disposal.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 5,
                   UserId = 1,
                   CategoryId = 3,
                   ProductName = "Eco-Friendly Sponges",
                   ProductAddress = "12 Västerlånggatan, Stockholm",
                   ProductQuantity = 80,
                   ProductQuality = "High",
                   ProductPrice = 9.99m,
                   ProductRegion = "Sweden",
                   ProductImageUrl = "/Uploads/products/item-5.jpg",
                   ProductDescription = "Clean your home sustainably with our Eco-Friendly Sponges. Made from natural cellulose and loofah materials, these sponges are biodegradable and durable. Ideal for kitchen and bathroom use, they provide effective cleaning without leaving microplastics behind. A responsible choice for households seeking greener cleaning alternatives.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 6,
                   UserId = 1,
                   CategoryId = 3,
                   ProductName = "Organic Cotton Kitchen Towels",
                   ProductAddress = "Elm Street 8, London",
                   ProductQuantity = 80,
                   ProductQuality = "High",
                   ProductPrice = 17.49m,
                   ProductRegion = "United Kingdom",
                   ProductImageUrl = "/Uploads/products/item-6.jpg",
                   ProductDescription = "Upgrade your kitchen essentials with our Organic Cotton Kitchen Towels, crafted from 100% certified organic cotton. These towels are soft, durable, and highly absorbent, making them perfect for drying dishes, cleaning spills, or everyday use. Free from harmful dyes and chemicals, they are a safe and eco-friendly alternative to synthetic materials. Designed to last and easy to wash, they combine functionality with sustainability for a cleaner kitchen and a healthier planet.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 7,
                   UserId = 1,
                   CategoryId = 4,
                   ProductName = "Organic Cotton T-shirt",
                   ProductAddress = "Sunset Boulevard 10, Los Angeles",
                   ProductQuantity = 150,
                   ProductQuality = "Premium",
                   ProductPrice = 22.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-7.jpg",
                   ProductDescription = "Wear comfort and sustainability with our Organic Cotton T-shirt, made from 100% certified organic cotton. This t-shirt is soft, breathable, and designed for long-lasting comfort. Ideal for everyday wear, it’s a perfect blend of style and eco-consciousness. Free from synthetic chemicals and dyes, it’s gentle on the skin and kind to the environment. Available in various sizes, this t-shirt combines timeless fashion with a commitment to sustainability.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 8,
                   UserId = 1,
                   CategoryId = 4,
                   ProductName = "Hemp Shoes",
                   ProductAddress = "Eco Street 50, Berlin",
                   ProductQuantity = 150,
                   ProductQuality = "High",
                   ProductPrice = 59.99m,
                   ProductRegion = "Germany",
                   ProductImageUrl = "/Uploads/products/item-8.jpg",
                   ProductDescription = "Step into sustainability with our Hemp Shoes, crafted from durable and eco-friendly hemp fabric. These shoes are designed to provide both comfort and style, while being gentle on the environment. Lightweight, breathable, and perfect for everyday wear, they are a great choice for those who want to reduce their carbon footprint. The hemp material is naturally resistant to wear and tear. Ideal for conscious consumers who value both fashion and sustainability.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 9,
                   UserId = 1,
                   CategoryId = 5,
                   ProductName = "Natural Lip Balm",
                   ProductAddress = "Maple Lane 12, Toronto",
                   ProductQuantity = 80,
                   ProductQuality = "High",
                   ProductPrice = 8.49m,
                   ProductRegion = "Canada",
                   ProductImageUrl = "/Uploads/products/item-9.jpg",
                   ProductDescription = "Protect and nourish your lips with our Natural Lip Balm, made from organic beeswax, shea butter, and essential oils. Free from synthetic additives and preservatives, it offers a smooth, moisturizing experience that soothes dry or chapped lips. Compact and easy to carry, it's perfect for daily use and suitable for all skin types. A gentle, eco-friendly choice for healthy lips all year round.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 10,
                   UserId = 1,
                   CategoryId = 5,
                   ProductName = "Organic Face Serum",
                   ProductAddress = "Botanic Boulevard 22, Vienna",
                   ProductQuantity = 60,
                   ProductQuality = "Premium",
                   ProductPrice = 39.99m,
                   ProductRegion = "Austria",
                   ProductImageUrl = "/Uploads/products/item-10.jpg",
                   ProductDescription = "Revitalize your skin with our Organic Face Serum, enriched with powerful botanical extracts, antioxidants, and essential oils. This lightweight serum penetrates deeply to hydrate, repair, and restore your skin’s natural glow. Free from parabens, silicones, and synthetic fragrances, it’s ideal for daily use and suitable for all skin types. Embrace radiant, healthy skin with every drop of this eco-conscious skincare essential.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 11,
                   UserId = 1,
                   CategoryId = 6,
                   ProductName = "Plant-Based Multivitamins",
                   ProductAddress = "Herbal Plaza 12, Amsterdam",
                   ProductQuantity = 60,
                   ProductQuality = "High",
                   ProductPrice = 24.99m,
                   ProductRegion = "Netherlands",
                   ProductImageUrl = "/Uploads/products/item-11.jpg",
                   ProductDescription = "Support your health naturally with our Plant-Based Multivitamins. Formulated using organic fruits, vegetables, and herbs, these supplements deliver essential nutrients without artificial additives. Designed to boost immunity, energy levels, and overall wellness, they are a great daily choice for those following a vegan or eco-conscious lifestyle. Free from GMOs, synthetic fillers, and animal products.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 12,
                   UserId = 1,
                   CategoryId = 6,
                   ProductName = "Detox Tea Blends",
                   ProductAddress = "Wellness Road 12, Vancouver",
                   ProductQuantity = 120,
                   ProductQuality = "High",
                   ProductPrice = 18.49m,
                   ProductRegion = "Canada",
                   ProductImageUrl = "/Uploads/products/item-12.jpg",
                   ProductDescription = "Revitalize your body with our Detox Tea Blends, carefully crafted with a mix of natural herbs like dandelion root, ginger, and green tea. This blend helps support digestion, flush toxins, and boost energy levels naturally. Enjoy a soothing cup daily to feel refreshed and balanced. 100% organic, caffeine-free, and free from artificial additives.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 13,
                   UserId = 1,
                   CategoryId = 7,
                   ProductName = "Eco-Friendly Spray Paint",
                   ProductAddress = "Maple Street 22, Toronto",
                   ProductQuantity = 80,
                   ProductQuality = "High",
                   ProductPrice = 11.49m,
                   ProductRegion = "Canada",
                   ProductImageUrl = "/Uploads/products/item-13.jpeg",
                   ProductDescription = "Add vibrant color with our Eco-Friendly Spray Paint, designed with low-VOC and water-based formulas. Perfect for DIY projects, crafts, or touch-ups, it provides strong coverage while being safer for you and the environment. Quick-drying, odor-reduced, and compliant with environmental safety standards, it's a smarter choice for sustainable creativity.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 14,
                   UserId = 1,
                   CategoryId = 7,
                   ProductName = "Biodegradable Sticker Remover",
                   ProductAddress = "Ocean Drive 7, Sydney",
                   ProductQuantity = 60,
                   ProductQuality = "Medium",
                   ProductPrice = 8.75m,
                   ProductRegion = "Australia",
                   ProductImageUrl = "/Uploads/products/item-14.jpg",
                   ProductDescription = "Easily remove stubborn stickers and adhesive residue with our Biodegradable Sticker Remover. Made from plant-based ingredients, it's non-toxic and safe to use on a variety of surfaces including glass, plastic, and metal. A perfect eco-alternative to harsh chemical removers, leaving no harmful traces behind.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 15,
                   UserId = 1,
                   CategoryId = 8,
                   ProductName = "Portable Solar Charger",
                   ProductAddress = "Solar Park 14, San Diego",
                   ProductQuantity = 60,
                   ProductQuality = "High",
                   ProductPrice = 49.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-15.jpg",
                   ProductDescription = "Stay powered anywhere with our Portable Solar Charger. Designed for outdoor enthusiasts and travelers, this lightweight and compact charger uses solar energy to charge smartphones, tablets, and other USB-powered devices. Built with high-efficiency solar panels, it’s perfect for camping, hiking, or emergency use. Durable, water-resistant, and eco-friendly, this solar charger is your reliable power source on the go.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 16,
                   UserId = 1,
                   CategoryId = 8,
                   ProductName = "Solar-Powered Garden Lights",
                   ProductAddress = "Sunshine Road 18, Lisbon",
                   ProductQuantity = 80,
                   ProductQuality = "High",
                   ProductPrice = 39.90m,
                   ProductRegion = "Portugal",
                   ProductImageUrl = "/Uploads/products/item-16.jpeg",
                   ProductDescription = "Illuminate your outdoor spaces with our Solar-Powered Garden Lights. These eco-friendly lights charge during the day and automatically light up at night, creating a warm and inviting atmosphere without any wiring or electricity bills. Ideal for pathways, patios, or gardens.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 17,
                   UserId = 1,
                   CategoryId = 9,
                   ProductName = "Compostable Paper Plates",
                   ProductAddress = "Eco Market 22, Amsterdam",
                   ProductQuantity = 200,
                   ProductQuality = "High",
                   ProductPrice = 6.49m,
                   ProductRegion = "Netherlands",
                   ProductImageUrl = "/Uploads/products/item-17.jpg",
                   ProductDescription = "Serve food sustainably with our Compostable Paper Plates. Made from natural, biodegradable fibers like sugarcane, they are sturdy, grease-resistant, and break down completely after use. A perfect solution for eco-conscious parties, picnics, or catering services without compromising on convenience.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 18,
                   UserId = 1,
                   CategoryId = 9,
                   ProductName = "Biodegradable Cutlery Set",
                   ProductAddress = "Greenway Avenue 12, Berlin",
                   ProductQuantity = 150,
                   ProductQuality = "Medium",
                   ProductPrice = 9.99m,
                   ProductRegion = "Germany",
                   ProductImageUrl = "/Uploads/products/item-18.jpg",
                   ProductDescription = "Eco-friendly biodegradable cutlery set including a fork, knife, reusable straw, and sushi chopsticks, perfect for outdoor events, picnics, or everyday use. Made from sustainable plant-based materials, this set offers a great alternative to plastic utensils, ensuring minimal environmental impact. Biodegradable and compostable, it is a step towards reducing waste and living sustainably.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 19,
                   UserId = 1,
                   CategoryId = 10,
                   ProductName = "VoltRide Electric Scooter",
                   ProductAddress = "Mobility Street 7, Berlin",
                   ProductQuantity = 25,
                   ProductQuality = "Premium",
                   ProductPrice = 499.99m,
                   ProductRegion = "Germany",
                   ProductImageUrl = "/Uploads/products/item-19.jpg",
                   ProductDescription = "Experience urban mobility with VoltRide, the eco-friendly electric scooter perfect for fast, emission-free commuting. Featuring a quiet 350W motor, up to 30 km range, and a foldable design, VoltRide is the ideal choice for daily city rides. Equipped with LED headlight, efficient brakes, and a durable build, it delivers both safety and comfort in a green solution.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 20,
                   UserId = 1,
                   CategoryId = 10,
                   ProductName = "Eco-Friendly Car Seat Covers",
                   ProductAddress = "Green Mobility Blvd 12, Los Angeles",
                   ProductQuantity = 40,
                   ProductQuality = "Premium",
                   ProductPrice = 39.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-20.jpg",
                   ProductDescription = "Upgrade your ride with our Eco-Friendly Car Seat Covers, crafted from sustainable materials like organic cotton and recycled PET fibers. Designed to offer comfort, durability, and a reduced environmental footprint, these covers fit most car models and help protect your vehicle’s interior with style and conscience. Machine washable and free from harmful chemicals, they’re the perfect choice for green driving enthusiasts.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 21,
                   UserId = 1,
                   CategoryId = 11,
                   ProductName = "Microfiber Car Cleaning Mitt",
                   ProductAddress = "Auto Eco Center, Detroit",
                   ProductQuantity = 200,
                   ProductQuality = "Premium",
                   ProductPrice = 6.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-21.jpg",
                   ProductDescription = "Keep your car spotless with our Microfiber Car Cleaning Mitt. Designed to trap dust, dirt, and grime without scratching your vehicle's surface, this eco-friendly mitt is washable and reusable. Perfect for both dry dusting and wet cleaning, it ensures a shiny finish with minimal effort. A must-have for sustainable car maintenance.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 22,
                   UserId = 1,
                   CategoryId = 11,
                   ProductName = "Biodegradable Air Freshener",
                   ProductAddress = "Eco Drive 88, San Francisco",
                   ProductQuantity = 150,
                   ProductQuality = "Premium",
                   ProductPrice = 6.49m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-22.jpg",
                   ProductDescription = "Refresh your space with our Biodegradable Air Freshener, made from natural essential oils and compostable materials. Designed to neutralize odors without harming the environment, it’s perfect for cars, offices, and homes. Its sleek design and long-lasting fragrance make it a smart and sustainable choice for everyday freshness.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 23,
                   UserId = 1,
                   CategoryId = 12,
                   ProductName = "Hempcrete Blocks",
                   ProductAddress = "Sustainable Building Street 8, Berlin",
                   ProductQuantity = 2500,
                   ProductQuality = "Premium",
                   ProductPrice = 3.25m,
                   ProductRegion = "Germany",
                   ProductImageUrl = "/Uploads/products/item-23.jpg",
                   ProductDescription = "Build responsibly with Hempcrete Blocks — an eco-friendly alternative to traditional concrete, made from hemp hurds, lime, and water. These blocks provide natural insulation, regulate humidity, and are resistant to mold and pests. Lightweight yet durable, they are ideal for sustainable construction projects focused on energy efficiency and reducing carbon footprint.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 24,
                   UserId = 1,
                   CategoryId = 12,
                   ProductName = "Bamboo Flooring",
                   ProductAddress = "Eco Construction Blvd 12, San Francisco",
                   ProductQuantity = 3000,
                   ProductQuality = "Premium",
                   ProductPrice = 30.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-24.jpg",
                   ProductDescription = "Upgrade your home with our Bamboo Flooring, a durable and sustainable alternative to hardwood. Made from rapidly renewable bamboo, this flooring features a smooth finish, high resistance to wear, and a natural look that complements any space. It’s easy to install and maintain, making it ideal for eco-conscious home renovations.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 25,
                   UserId = 1,
                   CategoryId = 13,
                   ProductName = "Organic Pet Food",
                   ProductAddress = "Green Paws Lane 45, Los Angeles",
                   ProductQuantity = 250,
                   ProductQuality = "Premium",
                   ProductPrice = 24.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-25.jpg",
                   ProductDescription = "Give your furry friend the best with our Organic Pet Food. Made from 100% natural ingredients, this food is free from artificial additives, preservatives, and hormones. Packed with essential nutrients, vitamins, and minerals, it supports the overall health of your pet while promoting a shiny coat and strong immune system. Perfect for dogs and cats with sensitive stomachs.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 26,
                   UserId = 1,
                   CategoryId = 13,
                   ProductName = "Natural Pet Shampoo",
                   ProductAddress = "Paws Care Street 12, Los Angeles",
                   ProductQuantity = 250,
                   ProductQuality = "Premium",
                   ProductPrice = 19.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-26.jpg",
                   ProductDescription = "Treat your pet to a gentle yet effective cleanse with our Natural Pet Shampoo. Made with plant-based ingredients and essential oils, it’s perfect for pets with sensitive skin. Free from harsh chemicals, this shampoo hydrates and soothes the skin, leaving your pet feeling fresh and smelling delightful. Safe for both dogs and cats, it’s a natural way to maintain a clean and healthy coat.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 27,
                   UserId = 1,
                   CategoryId = 14,
                   ProductName = "Organic Vegetable Seeds",
                   ProductAddress = "Green Thumb Lane 18, Denver",
                   ProductQuantity = 500,
                   ProductQuality = "High",
                   ProductPrice = 7.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-27.jpg",
                   ProductDescription = "Grow your own organic vegetables with our high-quality Organic Vegetable Seeds. These non-GMO seeds are perfect for home gardeners looking to cultivate a variety of fresh and healthy vegetables. Free from chemicals and pesticides, they offer a sustainable and eco-friendly way to enjoy home-grown produce. Ideal for small and large gardens alike.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 28,
                   UserId = 1,
                   CategoryId = 14,
                   ProductName = "Biodegradable Plant Pots",
                   ProductAddress = "Green Leaf Street 12, San Francisco",
                   ProductQuantity = 150,
                   ProductQuality = "High",
                   ProductPrice = 9.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-28.jpg",
                   ProductDescription = "Grow your plants in an eco-friendly way with our Biodegradable Plant Pots. Made from sustainable materials, these pots break down naturally over time, allowing your plants to grow without harming the environment. Perfect for starting seeds or growing small plants, these biodegradable pots help reduce plastic waste and promote a greener, cleaner planet.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 29,
                   UserId = 1,
                   CategoryId = 15,
                   ProductName = "Bamboo Pens",
                   ProductAddress = "Eco Street 5, Los Angeles",
                   ProductQuantity = 200,
                   ProductQuality = "Medium",
                   ProductPrice = 1.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-29.jpg",
                   ProductDescription = "Write with a conscience using our Bamboo Pens. Crafted from sustainable bamboo, these pens are not only stylish but also eco-friendly. With a smooth writing experience, they are perfect for everyday use while contributing to reducing plastic waste. The bamboo material makes these pens a great alternative for environmentally-conscious individuals.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               },

               new ProductDbTable
               {
                   Id = 30,
                   UserId = 1,
                   CategoryId = 15,
                   ProductName = "Recycled Paper Notebooks",
                   ProductAddress = "Green Avenue 22, San Francisco",
                   ProductQuantity = 150,
                   ProductQuality = "High",
                   ProductPrice = 6.99m,
                   ProductRegion = "United States",
                   ProductImageUrl = "/Uploads/products/item-30.jpg",
                   ProductDescription = "Take notes sustainably with our Recycled Paper Notebooks. Made from 100% recycled paper, these notebooks are perfect for those who want to reduce waste while maintaining a high-quality writing experience. Whether for work, school, or personal journaling, these notebooks are durable, eco-friendly, and a great step towards a more sustainable lifestyle.",
                   ProductPostDate = DateTime.Now,
                   ProductStatus = ProductStatus.Available,
                   RecommendationStatus = RecommendationStatus.Preferred,
                   ProductRating = 5
               }
            );

            context.SaveChanges();
        }
    }
}
