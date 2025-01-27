﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TrendOl.Entities;

namespace TrendOl.DataAccessLayer
{
	class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
	{
		protected override void Seed(DatabaseContext context)
		{
			List<Brand> brandList = new List<Brand>();
			List<MyUser> brandUsers = new List<MyUser>();
			List<Product> productList = new List<Product>();


			//Only one super user is creating
			MyUser SuperUser = new MyUser()
			{
				Name = "Emre",
				Surname = "Serbest",
				ActivateGuid = Guid.NewGuid(),
				Email = "eserbest1903@gmail.com",
				IsActive = true,
				IsSuperUser = true,
				HasBrand = false,
				Username = "eserbest",
				Password = "123456",
				UserImage = FakeData.NetworkData.GetDomain(),

			};

			context.MyUsers.Add(SuperUser);
			context.SaveChanges();


			//Creating 7 Admins (Brand Users)
			for (int i = 0; i < 7; i++)
			{
				MyUser BrandUser = new MyUser()
				{
					Name = FakeData.NameData.GetFirstName(),
					Surname = FakeData.NameData.GetSurname(),
					Email = FakeData.NetworkData.GetEmail(),
					UserImage = FakeData.NetworkData.GetDomain(),
					ActivateGuid = Guid.NewGuid(),
					IsActive = true,
					IsSuperUser = false,
					HasBrand = true,
					Username = $"brandUser{i}",
					Password = "b123"
				};
				brandUsers.Add(BrandUser);
				context.MyUsers.Add(BrandUser);
			}
				context.SaveChanges();


			//List<MyUser> users = context.MyUsers.ToList();
			//Creating a Brand for the current BrandUser
			foreach (MyUser item in brandUsers)
			{
				Brand newBrand = new Brand()
				{
					BrandName = FakeData.NameData.GetCompanyName(),
					BrandImage = "BrandImage.jpg",
					Tag = FakeData.TextData.GetSentence(),
					MyUser = item,
				};
				brandList.Add(newBrand);
				context.Brands.Add(newBrand);

			}
			context.SaveChanges();

			// Creating categories(3,6) for the current Brand

			foreach (var itemBrand in brandList)
			{
				for (int a = 0; a < FakeData.NumberData.GetNumber(3, 6); a++)
				{
					Category category = new Category()
					{
						Title = FakeData.NameData.GetSurname(),
						Description = FakeData.TextData.GetSentence(),
						Brand = itemBrand,
					};

					context.Categories.Add(category);
				}
			}
			context.SaveChanges();

			List<string> sizeList = new List<string>();
			sizeList.Add("XS");
			sizeList.Add("S");
			sizeList.Add("M");
			sizeList.Add("L");
			sizeList.Add("XL");
			sizeList.Add("XXL");

			List<string> colorList = new List<string>();
			colorList.Add("black");
			colorList.Add("white");
			colorList.Add("pink");
			colorList.Add("green");
			colorList.Add("red");


			//Creating 50 products
			for (int j=0; j<50; j++)
			{
				Product newProduct = new Product()
				{
					ProductName = FakeData.NameData.GetCompanyName(),
					ProductDescription = FakeData.TextData.GetSentence(),
					Brand = brandList[(FakeData.NumberData.GetNumber(0, brandList.Count - 1))],
					Size = sizeList[(FakeData.NumberData.GetNumber(0, sizeList.Count - 1))],
					Color = colorList[(FakeData.NumberData.GetNumber(0, colorList.Count - 1))],
					Stock = FakeData.NumberData.GetNumber(10, 100),
					Price = FakeData.NumberData.GetDouble(),
					DiscountPercentage = FakeData.NumberData.GetNumber(0,10),					

				};

				productList.Add(newProduct);
				context.Product.Add(newProduct);

			}
			context.SaveChanges();


			List<MyUser> standartUserList = new List<MyUser>();

			for (int s = 0; s < 10; s++)
			{
				MyUser standartUser = new MyUser()
				{
					Name = FakeData.NameData.GetFirstName(),
					Surname = FakeData.NameData.GetSurname(),
					Email = FakeData.NetworkData.GetEmail(),
					UserImage = FakeData.NetworkData.GetDomain(),
					ActivateGuid = Guid.NewGuid(),
					IsActive = true,
					IsSuperUser = false,
					HasBrand = false,
					Username = $"standartUser{s}",
					Password = "s123"
				};
				standartUserList.Add(standartUser);
				context.MyUsers.Add(standartUser);
				context.SaveChanges();

				//Creating Wishlist for standartusers randomly.
				if (FakeData.BooleanData.GetBoolean())
				{
					Wishlist wishlist = new Wishlist()
					{
						IsPublic = FakeData.BooleanData.GetBoolean(),
						Title = FakeData.BooleanData.GetBoolean() ? "Specialized-WishList-Title" : "WishList",
						MyUser = standartUser,
					};
					context.Wishlists.Add(wishlist);
					
					//Adding products to the wishlist randomly
					for (int i = 0; i < FakeData.NumberData.GetNumber(3,10); i++)
					{
						Wishlist_Products wishlist_products = new Wishlist_Products()
						{
							Product = productList[FakeData.NumberData.GetNumber(0,45)],
							Wishlist = wishlist
						};
						context.WishlistProduct.Add(wishlist_products);

					}
				}

				//Creating Sale for standart users randomly
				if (FakeData.BooleanData.GetBoolean())
				{

					for (int salecounter = 0; salecounter < FakeData.NumberData.GetNumber(3,20); salecounter++)
					{
						double totalPrice = 0;

						Sale_Details saleDetail = new Sale_Details()
						{ };

						Sale sale = new Sale()
						{ };
						for (int saleDetailCounter = 0; saleDetailCounter < FakeData.NumberData.GetNumber(2,8); saleDetailCounter++)
						{
							Product soldProduct = productList[FakeData.NumberData.GetNumber(0, 49)];
							int tempQuantity = FakeData.NumberData.GetNumber(1, 4);
							double tempSubTotal = soldProduct.Price * tempQuantity;
							totalPrice += tempSubTotal;


							saleDetail.Quantity = tempQuantity;
							saleDetail.Product = soldProduct;
							saleDetail.InstantPrice = soldProduct.Price;
							saleDetail.SubTotal = tempSubTotal;
							saleDetail.Sale = sale;
							

							context.Sale_Details.Add(saleDetail);
							
						}


						sale.Customer = standartUser;
						sale.Date = FakeData.DateTimeData.GetDatetime();
						sale.Time = FakeData.DateTimeData.GetDatetime();
						sale.TotalPrice = totalPrice;
						sale.TotalVat = 18;
						
						

						context.Sales.Add(sale);
					}

				}	


			}

			context.SaveChanges();

			

			//Adding product images
			foreach (Product itemProduct in productList)
			{
				for (int i = 0; i < FakeData.NumberData.GetNumber(3,7); i++)
				{
					Product_Image productImage = new Product_Image()
					{
						Url = $"picture{i}.jpg",
						Product = itemProduct,
					};
					context.ProductImages.Add(productImage);

				
					//Adding comments
					Comment newComment = new Comment()
					{
						Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
						Owner = standartUserList[FakeData.NumberData.GetNumber(0, standartUserList.Count - 1)],
						Product = itemProduct,
					};
					context.Comments.Add(newComment);


					//Creating likes
					Like like = new Like()
					{
						LikeOwner = standartUserList[FakeData.NumberData.GetNumber(0, standartUserList.Count - 1)],
						Product = itemProduct,
					};
					context.Likes.Add(like);
				}
			}
			context.SaveChanges();	

		}

	}
}
