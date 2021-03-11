using Critic.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using static Critic.Models.AppUser;
using Microsoft.EntityFrameworkCore;

namespace Critic.Data
{
    public static  class DbInitializer
    {

        public static void Initialize(CriticDbContext context, IConfiguration configuration)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [User] ON");

                var users = new AppUser[]
                {
                    new AppUser {Id= 1, Email = configuration["DefaultAdminEmail"], Role=Roles.Admin, Image="https://lh3.googleusercontent.com/a-/AOh14GhpsvkAGYxYE1T381ofkBDOs3fi7pO7Uuxfxc46=s96-c", Name = "Diego Parra"},
                    new AppUser {Id= 2, Email = "yocara@gmail.com", Role=Roles.Owner, Image="https://lh3.googleusercontent.com/a-/AOh14GjS49-AxHhqUC2UBIMqqHJ-AkRYy2zt92flWA7T=s96-c", Name = "Carolina Ramirez"},
                    new AppUser {Id= 3, Email = "monica.geller@fakegmail.com", Role=Roles.Owner, Image="https://upload.wikimedia.org/wikipedia/en/d/d0/Courteney_Cox_as_Monica_Geller.jpg", Name = "Monica Geller"},
                    new AppUser {Id= 4, Email = "owner.critic@gmail.com", Role=Roles.Owner, Image="https://lh3.googleusercontent.com/a-/AOh14Gg5HyyJJ67wVj02K4da1GeGvqPTbLS36NkSCWay=s96-c", Name = "Owner - Critic"},
                    new AppUser {Id= 5, Email = "joey.tribbiani@fakegmail.com", Role=Roles.User, Image="https://i.pinimg.com/originals/70/88/a7/7088a7a7d8058d9d520f700ddccb7196.jpg", Name = "Sam Adams"},
                    new AppUser {Id= 6, Email = "rachel.green@fakegmail.com", Role=Roles.User, Image="https://images.saymedia-content.com/.image/c_limit%2Ccs_srgb%2Cq_auto:good%2Cw_700/MTc1MTE0MDUxOTg5MzQ5NDcx/which-friends-character-was-the-greatest.webp", Name = "Rachel Green"},
                    new AppUser {Id= 7, Email = "chandler.bing@fakegmail.com", Role=Roles.User, Image="https://upload.wikimedia.org/wikipedia/en/6/66/Matthew_Perry_as_Chandler_Bing.png", Name = "Chandler Bing"},
                    new AppUser {Id= 8, Email = "ross.geller@fakegmail.com", Role=Roles.User, Image="https://i.pinimg.com/originals/c5/f6/2a/c5f62a7246ffdde15c2e97c6151ade75.jpg", Name = "Ross Geller"},
                    new AppUser {Id= 9, Email = "phoebe.buffay@fakegmail.com", Role=Roles.User, Image="https://hips.hearstapps.com/digitalspyuk.cdnds.net/14/36/ustv-friends-10.jpg?crop=1.00xw:0.735xh;0,0.0217xh&resize=480:*", Name = "Phoebe Buffay"},
                    new AppUser {Id= 10, Email = "userX.critic@fakegmail.com", Role=Roles.User, Image="https://lh3.googleusercontent.com/a-/AOh14GjDPKFlpDw5hEJk3ZwL4z0hARDm5hBulUHVv2yq=s96-c", Name = "User X Critic"},
                };
                context.Users.AddRange(users);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [User] OFF");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Restaurant ON");
                var restaurants = new Restaurant[]
                {
                    new Restaurant { Id= 1, Name = "Criterion", Price = 60, Rating = (decimal)4.33, OwnerID = 2, Description = "The renowned Rausch brothers have opened this fine dining address in the trendy zona G district and has become a well-known classic in Colombia. The dining room accomplishes a modern and inviting atmosphere. Its contemporary French cuisine: delicate, sophisticated, true to the new-age techniques and best ingredients, is what has earned them numerous awards and recognition. The tasting menu, with Colombian notes, is a great success.", City = "Bogota", Address = "Calle 69A # 5 - 75", Image= "https://res.cloudinary.com/tf-lab/image/upload/restaurant/ebee68d9-6901-4850-b4c2-18d5c90a1770/9a717455-7694-421e-854f-7a1d08128ede.jpg"},
                    new Restaurant { Id= 2, Name = "Di Lucca", Price = 15, Rating = (decimal)3.66, OwnerID = 2, Description = "Di Lucca restaurant is over 26 years old and continues to be one of the most traditional and recognized Italian restaurants in Bogotá. It is synonymous with quality and good service, always being a good option. At this moment we have 3 offices: Bogotá 85, Salitre and Chía.", City = "Chia", Address = "Kilometro 2 vía Chía-Cajica", Image= "https://media-cdn.tripadvisor.com/media/photo-p/15/5e/8c/71/photo1jpg.jpg"},
                    new Restaurant { Id= 3, Name = "Celestina", Price = 15, Rating = (decimal)4, OwnerID = 2, Description = "A neighborhood restaurant in Cajicá that opened its doors in a cozy rustic house with a different proposal. A place to enjoy eating and sharing around a table. The highlight of the menu are dishes made in a wood oven, which combine the international with the artisanal. The dishes vary with the seasons; You will find favorites like Brontosaurus beef, crab legs, smoked trout or freshly made pasta. Everyone will leave happier than when they arrived.", City = "Chia", Address = "Via Cajicá Chía, al frente de Fontanar", Image= "https://asset3.zankyou.com/images/wervice-card/398/a610/400/312/w/824092/-/[2x]-1583183364.jpg.webp"},
                    new Restaurant { Id= 4, Name = "Mirazur", Price = 15, Rating = 5, OwnerID = 3,  Description = "Unrivalled views of the French Riviera, three levels of cascading vegetable gardens churning out the sweetest produce and a team of outrageously talented cooks and front-of-house staff combine to make Mirazur the ultimate restaurant experience. Mauro Colagreco’s unique cuisine is inspired by the sea, the mountains and the restaurant’s own gardens, including Menton’s emblematic citrus fruits", City = "Menton", Address = "30 Avenue Aristide Briand", Image= "https://www.theworlds50best.com/filestore/jpg/Mirazur-WORLD-2019-INTERIOR.jpg"},
                    new Restaurant { Id= 5, Name = "Noma", Price = 100, Rating = 4, OwnerID = 3, Description = "The original Noma was, undoubtedly, one of the most important restaurants of its generation. With his food, René Redzepi developed a new genre of cuisine. New Nordic cookery looks back to look ahead; digging deeper than seasonality to explore unsung foraged products, while seamlessly weaving in a study of fermentation. Redzepi’s visionary approach to celebrating terroir via ingredient-focused, minimalist plates earned the first incarnation of Noma the title of The World’s Best Restaurant in four years: 2010, 2011, 2012, and 2014.", City = "Copenhagen", Address = "Refshalevej 96", Image= "https://www.theworlds50best.com/filestore/jpg/Noma-WORLD-2019-INTERIOR.jpg"},
                    new Restaurant { Id= 6, Name = "Asador Etxebarri", Price = 90, Rating = 5, OwnerID = 3, Description = "The restaurant respects the intrinsic natural flavours of local produce and delicately urges each ingredient to show its potential: goat’s milk churned into ethereal butter, green peas amplified in their own juice, beef dry aged for so many days it bites with umami. Arguinzoniz cooks vegetables and proteins on a range of charcoals he makes from a variety of woods, kissing most plates with at least a suggestion of smoke", City = "Axpe", Address = "Plaza de San Juan 1", Image= "https://www.theworlds50best.com/filestore/jpg/Asador-WORLD-2019-EXTERIOR.jpg"},
                    new Restaurant { Id= 7, Name = "Gaggan", Price = 50, Rating = 5, OwnerID = 4, Description = "For four years in a row (2014-2018), Gaggan was voted No.1 in Asia’s 50 Best Restaurants, a testament to the constant innovation and improvement at this ever-evolving hub of creativity. El Bulli-influenced chef Gaggan Anand serves up a menu of 25 or more courses of rapid-fire small bites, many of which are eaten with the hands.", City = "Bangkok", Address = "68/1 Soi Langsuan, Ploenchit Road, Lumpini", Image= "https://www.theworlds50best.com/filestore/jpg/Gaggan-WORLD-2019-INTERIOR.jpg"},
                    new Restaurant { Id= 8, Name = "Geranium", Price = 120, Rating = 4, OwnerID = 4, Description = "The seemingly unlikely duo of nature and technology are at the heart of chef Rasmus Kofoed’s progressive tasting menu: 17-plus inspired, artistic courses composed of organic and wild Scandinavian ingredients. While a presentation of fragile, near-translucent leaves is made from a Jerusalem artichoke purée, what look to be razor clams are actually dough painted with squid ink.", City = "Copenhagen", Address = "Per Henrik Lings Allé 4, 8", Image= "https://www.theworlds50best.com/filestore/jpg/Geranium-WORLD-2019-INTERIOR.jpg"},
                    new Restaurant { Id= 9, Name = "Grotta Palazzese", Price = 100, Rating = (decimal)3.66, OwnerID = 4, Description = "A magical and enchanted place, a restaurant with a terrace created inside a natural cave. The Grotta Palazzese restaurant takes its name from the homonymous cave and from the place that was once also called 'Grotta di Palazzo'. It is an exclusive and atmospheric place, used for parties and banquets since 1700, as evidenced by a watercolor of 1783 by Jean Louis Desprez. The blue of the sea and the sky contrasts with the mysterious atmospheres of the natural cavities. The colors of the day make the beauty of the coastal landscape shine, while those of the evening, at sunset, create unforgettable nuances, in the charm and elegance of the restaurant lights.", City = "Polignano a Mare", Address = "Via Narciso, 59", Image= "https://www.grottapalazzese.it/wp-content/uploads/2018/03/grotta-palazzese-tavolo-small.jpg"},
                };
                context.Restaurants.AddRange(restaurants);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Restaurant OFF");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Review ON");
                var reviews = new Review[] {
                    new Review { Id= 1, Date = new DateTime(2018,05, 29), Rating = (decimal) 4, RestaurantID = 1, UserID = 5, ReplyID = 1, Comment = "My niece and I order their degustations menu with corresponding pairing. I was a great experience for all the senses. I strongly recommend it if you enjoy experiencing fusion of French food and international food with Colombian ingredients, presented in a very contemporary way." },
                    new Review { Id= 2, Date = new DateTime(2019,08, 13), Rating = (decimal) 4, RestaurantID = 1, UserID = 6, ReplyID = 2, Comment = "Yesterday I had a business lunch at this restaurant. It was my first visit and I had big expectations. The facilities are nice, the staff knowledgeable and professional. The food was absolutely great. For main I had the pork which was maybe one if the best I’ve ever had.A couple of my colleagues had the lamb which was superbly displayed on the plate and delicious.We did not have desserts for dietary reasons, although they looked so appetizing. My rating of 4 / 5 stars come because such a good and expensive place can’t bring a coffee(a cappuccino) so below the standard.Silly detail, but with the level of exigency you need to have for such a place, this is not acceptable. " },
                    new Review { Id= 3, Date = new DateTime(2020, 02, 02), Rating = (decimal) 5, RestaurantID = 1, UserID = 7, ReplyID = 3, Comment = "This restaurant serves superb food. My husband regretted not having the tasting menu, but we were still too jet legged to have a full dinner experience. We had main courses and desserts and the food lived up to its reputation. We had an amazing bouillabaisse, a risotto with truffle, lamb and meat cooked to perfection and then a vanilla crème brûlée and two mille-feuilles ( one with Nutella) that were delicious. Good presentation and service. The restaurant could do with a different decor and the atmosphere did not match the food, but a restaurant that should be visited when in town." },
                    new Review { Id= 4, Date = new DateTime(2018, 05, 29), Rating = (decimal) 3, RestaurantID = 2, UserID = 8, ReplyID = 4, Comment = "We were so pleasantly surprised with how good the service was and how delicious and authentic the food was. We got the cheese pizza and it tasted just like the pizza we had in Italy and we later found out that the chef is from Italy. We were so thrilled that we finally found a pizza in Bogota where they actually put a good amount of cheese on the pizza. We also got the fried calamari, Greek salad, and chicken Florentine. Everything was so fresh and delicious. The calamari was perfectly crisp without being greasy and the chicken Florentine was so tender. It’s a pricer restaurant for Bogota but it’s worth it for the quality of food and amazing service. We can’t wait to go back!" },
                    new Review { Id= 5, Date = new DateTime(2019, 08, 13), Rating = (decimal) 4, RestaurantID = 2, UserID = 9, ReplyID = 5, Comment = "Wasn’t expecting much for a Sunday night when most places are closed, but our concierge recommended Di Lucca. It was perfect romantic but buzzy ambiance. We were seated quickly without reservation. Sat outside. Food wasn’t stellar but atmosphere made it the perfect spot for us." },
                    new Review { Id= 6, Date = new DateTime(2020, 02, 02), Rating = (decimal) 4, RestaurantID = 2, UserID = 9, ReplyID = 6, Comment = "DiLucca is a long time restaurant set for italian specialities, after many years conserve the quality on preparations, menu and most important service and visitors can relay that get want is expected from a fine restaurant, with sensible prices." },
                    new Review { Id= 7, Date = new DateTime(2020, 05, 18), Rating = (decimal) 4, RestaurantID = 3, UserID = 5, ReplyID = 7, Comment = "Good restaurant so far!! Totally recommended" },
                    new Review { Id= 8, Date = new DateTime(2020, 04, 21), Rating = (decimal) 5, RestaurantID = 4, UserID = 6, ReplyID = 8, Comment = "This is the best restaurant in the world, what can else can I say?" },
                    new Review { Id= 9, Date = new DateTime(2020, 02, 02), Rating = (decimal) 4, RestaurantID = 5, UserID = 7, ReplyID = 9, Comment = "Almost as perfect as Mirazur, However I never rate the perfect score because I am a very rare person" },
                    new Review { Id= 10, Date = new DateTime(2019, 01, 02), Rating = (decimal) 5, RestaurantID = 6, UserID = 8, ReplyID = 10, Comment = "Yummy!!! I wasn't expecting this kind of amazing service and delicious food!!!" },
                    new Review { Id= 11, Date = new DateTime(2021, 01, 20), Rating = (decimal) 5, RestaurantID = 7, UserID = 9, Comment = "We first attended Gaggan in 2017 at the previous location and wanted to experience the food in a conventional restaurant setting. However nothing about the new Gaggan Anand is conventional, the food is art on a plate and the service is pure theatre. Each dish of 18 and each wine of six was introduced to us by a chef or the sommelier. I lived in India for over three years and can confirm the authenticity of the flavours from the high quality ingredients. Anand and the team are doing a fantastic job." },
                    new Review { Id= 12, Date = new DateTime(2021, 01, 20), Rating = (decimal) 4, RestaurantID = 8, UserID = 9, Comment = "We had looked forward to visit Geranium for a very long time. Finally we got around to do it. It started off great. Service was impeccable. Drinks and wine were to die for. Every dish was a joy to look at and the taste was really amazing. But but but ... we felt that we were not welcome. The speed of the dishes rushing to our table was like being on a motorway. A full menu served in around 2,5 hour was way too fast. Really a shame." },
                    new Review { Id= 13, Date = new DateTime(2021, 01, 20), Rating = (decimal) 3, RestaurantID = 9, UserID = 5, Comment = "Amazing views but regular food, altough the salmon was really great!" },
                    new Review { Id= 14, Date = new DateTime(2021, 01, 20), Rating = (decimal) 4, RestaurantID = 9, UserID = 6, Comment = "The view and restaurant setting is breathtaking and the service is good. However the food is not worth the price due to the simplicity of the culinary experience. A little more creativity in the ingredients, flavours and dish design could help justify the price." },
                    new Review { Id= 15, Date = new DateTime(2021, 01, 20), Rating = (decimal) 5, RestaurantID = 9, UserID = 7, ReplyID = 11, Comment = "We booked for 6pm for dinner on a Wednesday so we could watched the sunset from the table. Even though the table was technically for two hours, the staff let us stay there the whole evening as it was a quiet night with only 3-4 other parties there. Alina (sp) and her team looked after us really well the entire evening." },
                    new Review { Id= 16, Date = new DateTime(2021, 01, 20), Rating = (decimal) 2, RestaurantID = 9, UserID = 8, Comment = "This unique location is absolutely breathtaking and magical . The magic disappears as soon as you have the deal with the staff, specially the door man and the gentleman at the cashier. They shouldn’t treat customers in such a despicable way, So much attitude and rudeness .The food is overpriced and nothing special.Only the location is worth your time and money. The rest is smoke and mirrors" },
                    new Review { Id= 17, Date = new DateTime(2021, 01, 20), Rating = (decimal) 3, RestaurantID = 9, UserID = 9, Comment = "Drive 4 hours to have lunch, and changed to make sure I met dress code; yet several with shorts and trainers. Car Park taxi only in the evening, not the best place to park. Not 5 star, and very over priced. Pasta was not cooked and flooded with Peter, other three dishes good. The waiters are not wine experts, service reasonable but again not 5 star. Go, enjoy setting but know another over priced place cashing in." },
                    new Review { Id= 18, Date = new DateTime(2021, 01, 20), Rating = (decimal) 5, RestaurantID = 9, UserID = 5, Comment = "The setting is absolutely breathtaking...the food was great, arguably not worth the expense of the bill, but we would say that you are paying here for the experience of the evening as a whole. It is expensive, but honestly as a once-in-a-lifetime visit for my boyfriend and I, we would say it’s absolutely worth it." }
                };
                context.Reviews.AddRange(reviews);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Review OFF");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Reply ON");
                var replies = new Reply[]{
                    new Reply { Id= 1, Date = new DateTime(2020,02,09), ReviewID = 1, UserID = 2, Comment = "Thank you for your visit and for your comment! Looking forward to have you again in our restaurant!"},
                    new Reply { Id= 2, Date = new DateTime(2020,02,09), ReviewID = 2, UserID = 2, Comment = "Thank you for your visit and for your comment! Looking forward to have you again in our restaurant!"},
                    new Reply { Id= 3, Date = new DateTime(2020,02,09), ReviewID = 3, UserID = 2, Comment = "Thank you for your visit and for your comment! Looking forward to have you again in our restaurant!"},
                    new Reply { Id= 4, Date = new DateTime(2020,02,09), ReviewID = 4, UserID = 2, Comment = "Thank you for your visit and for your comment! Looking forward to have you again in our restaurant!"},
                    new Reply { Id= 5, Date = new DateTime(2020,02,09), ReviewID = 5, UserID = 2, Comment = "Thank you for your visit and for your comment! Looking forward to have you again in our restaurant!"},
                    new Reply { Id= 6, Date = new DateTime(2020,02,09), ReviewID = 6, UserID = 2, Comment = "Thank you for your visit and for your comment! Looking forward to have you again in our restaurant!"},
                    new Reply { Id= 7, Date = new DateTime(2020,02,09), ReviewID = 7, UserID = 2, Comment = "Thank you for your visit and for your comment! Looking forward to have you again in our restaurant!"},
                    new Reply { Id= 8, Date = new DateTime(2020,02,09), ReviewID = 8, UserID = 3, Comment = "Hope you come again!!"},
                    new Reply { Id= 9, Date = new DateTime(2020,02,09), ReviewID = 9, UserID = 3, Comment = "Hope you come again!!"},
                    new Reply { Id= 10, Date = new DateTime(2020,02,09), ReviewID = 10, UserID = 3, Comment = "Hope you come again!!"},
                    new Reply { Id= 11, Date = new DateTime(2020,02,09), ReviewID = 15, UserID = 4, Comment = "Thank you for making all the way through here!"},
                };
                context.Replies.AddRange(replies);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Reply OFF");

                transaction.Commit();
            }
        }

    }
}
