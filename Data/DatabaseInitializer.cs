using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Thoughtwave.Models;

namespace Thoughtwave.Data
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(IApplicationBuilder app)
        {
            // Get database context
            ThoughtwaveDbContext context = app.ApplicationServices
                .GetRequiredService<ThoughtwaveDbContext>();

            context.Database.EnsureCreated();

            // Get user manager
            UserManager<User> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<User>>();

            // seed user data
            var user1 = await SeedUserDataAsync(userManager, 
                "genericuser1", "genericuser1@email.com", "Pa$$word1", 
                "Mary", "Hamilton", "https://randomuser.me/api/portraits/women/56.jpg");

            var user2 = await SeedUserDataAsync(userManager, 
                "genericuser2", "genericuser2@email.com", "Pa$$word2",
                "David", "Arnold", "https://randomuser.me/api/portraits/men/75.jpg");

            var user3 = await SeedUserDataAsync(userManager, 
                "genericuser3", "genericuser3@email.com", "Pa$$word3",
                "Elizabeth", "Lee", "https://randomuser.me/api/portraits/women/27.jpg");

            var user4 = await SeedUserDataAsync(userManager, 
                "genericuser4", "genericuser4@email.com", "Pa$$word4",
                "Johnny", "Walker", "https://randomuser.me/api/portraits/men/27.jpg");

            // Seed thought data
            if (!context.Thoughts.Any())
            {
                var Thoughts = new List<Thought>()
                {
                    new Thought()
                    {
                        Title = "How Can I Live Better",
                        Content = "Forage gochujang vape, mustache tumeric church-key master cleanse salvia godard hella hoodie everyday carry freegan. Sustainable forage aesthetic, neutra scenester lyft bespoke roof party taxidermy next level meggings coloring book. Jean shorts keffiyeh tacos migas normcore scenester.\n Fashion axe williamsburg lo-fi flexitarian unicorn ennui edison bulb.Forage gochujang vape, mustache tumeric church-key master cleanse salvia godard hella hoodie everyday carry freegan. Sustainable forage aesthetic, neutra scenester lyft bespoke roof party taxidermy next level meggings coloring book.\n Jean shorts keffiyeh tacos migas normcore scenester. Fashion axe williamsburg lo-fi flexitarian unicorn ennui edison bulb.",
                        Author = user3,
                        Category = Category.Film,
                        Comments = new List<Comment>()
                        {
                            new Comment() 
                            {  
                                Content = " Swag flannel kale chips microdosing church-key chia twee copper mug, unicorn hell of XOXO ethical letterpress fam. IPhone bitters gastropub austin, lo-fi semiotics kombucha forage coloring book ramps. Heirloom wolf trust fund flexitarian bicycle rights pop-up.",  
                                User = user2,
                            },
                            new Comment() 
                            {  
                                Content = "Snackwave 3 wolf moon tacos, fashion axe copper mug YOLO neutra disrupt hashtag vexillologist succulents. Four loko cardigan pop-up actually, salvia man braid banjo banh mi 3 wolf moon tumblr. Literally salvia neutra quinoa.", 
                                User = user3,
                            }
                        }
                    },
                    new Thought()
                    {
                        Title = "This New Band Is Amazing",
                        Content = "Bushwick truffaut pok pok, schlitz pinterest thundercats whatever vinyl tilde yuccie fixie fashion axe letterpress synth master cleanse. Umami pork belly before they sold out subway tile, craft beer air plant stumptown kombucha meditation. Thundercats marfa celiac hella banjo, af franzen polaroid.\n\n Prism raw denim vinyl, single-origin coffee plaid trust fund everyday carry kitsch pickled jianbing seitan fap. IPhone biodiesel lo-fi blue bottle, occupy live-edge distillery iceland pabst green juice wayfarers waistcoat. Sriracha subway tile fashion axe hoodie retro, offal mustache PBR&B selfies whatever bushwick beard vinyl. Gluten-free prism kitsch, umami listicle waistcoat tumblr subway tile VHS typewriter schlitz poke glossier squid.",
                        Author = user2,
                        Category = Category.Music,
                        Comments = new List<Comment>()
                        {
                            new Comment() 
                            {  
                                Content = "Kogi scenester iceland neutra polaroid tumeric snackwave craft beer. Authentic wolf man bun succulents messenger bag blog. Hexagon hoodie schlitz, celiac migas trust fund whatever.", 
                                User = user1,
                            },
                            new Comment() 
                            {  
                                Content = "Street art godard viral photo booth succulents hexagon. Occupy tumeric twee biodiesel, ", 
                                User = user3,
                            }
                        }
                    },
                    new Thought()
                    {
                        Title = "Should It Have Been Bernie?",
                        Content = "Asymmetrical viral bushwick schlitz flannel, raclette glossier yr sartorial plaid butcher direct trade kickstarter. Hell of plaid craft beer kinfolk, intelligentsia gochujang cardigan direct trade viral forage truffaut shoreditch slow-carb pinterest. Deep v jean shorts taxidermy hoodie migas.\n\n \tSquid selvage fanny pack synth single-origin coffee. Art party wolf live-edge helvetica gochujang, kickstarter slow-carb gluten-free shabby chic franzen selvage ethical. Af chicharrones asymmetrical, banh mi you probably haven't heard of them ramps synth dreamcatcher offal man braid irony four dollar toast. Twee hammock chambray organic.",
                        Author = user1,
                        Category = Category.Politics,
                        Comments = new List<Comment>()
                        {
                            new Comment() 
                            {  
                                Content = "Kogi scenester iceland neutra polaroid tumeric snackwave craft beer. Authentic wolf man bun succulents messenger bag blog. Hexagon hoodie schlitz, celiac migas trust fund whatever.", 
                                User = user1,
                            },
                            new Comment() 
                            {  
                                Content = "Street art godard viral photo booth succulents hexagon. Occupy tumeric twee biodiesel, ", 
                                User = user3,
                            }
                        }
                    },
                    new Thought()
                    {
                        Title = "The World Is NOT All Bad",
                        Content = "Distillery gastropub williamsburg, iceland sriracha dreamcatcher church-key. Viral flannel franzen, pug narwhal photo booth vape cliche mustache blog subway tile neutra selvage. Locavore vape heirloom meggings, williamsburg ugh etsy raclette. Jean shorts truffaut photo booth, schlitz farm-to-table master cleanse paleo. Cronut gluten-free activated charcoal, kombucha hell of plaid hashtag la croix unicorn bitters vinyl kickstarter slow-carb chambray retro. Jean shorts street art retro direct trade, keytar lyft iceland snackwave deep v ethical cliche 90's irony air plant poutine. Coloring book hammock raclette glossier umami yr, truffaut knausgaard forage bespoke mustache. Distillery gastropub williamsburg, iceland sriracha dreamcatcher church-key. Viral flannel franzen, pug narwhal photo booth vape cliche mustache blog subway tile neutra selvage. Locavore vape heirloom meggings, williamsburg ugh etsy raclette. Jean shorts truffaut photo booth, schlitz farm-to-table master cleanse paleo. Cronut gluten-free activated charcoal, kombucha hell of plaid hashtag la croix unicorn bitters vinyl kickstarter slow-carb chambray retro. Jean shorts street art retro direct trade, keytar lyft iceland snackwave deep v ethical cliche 90's irony air plant poutine. Coloring book hammock raclette glossier umami yr, truffaut knausgaard forage bespoke mustache. Distillery gastropub williamsburg, iceland sriracha dreamcatcher church-key. Viral flannel franzen, pug narwhal photo booth vape cliche mustache blog subway tile neutra selvage. Locavore vape heirloom meggings, williamsburg ugh etsy raclette. Jean shorts truffaut photo booth, schlitz farm-to-table master cleanse paleo.\n\n \tCronut gluten-free activated charcoal, kombucha hell of plaid hashtag la croix unicorn bitters vinyl kickstarter slow-carb chambray retro. Jean shorts street art retro direct trade, keytar lyft iceland snackwave deep v ethical cliche 90's irony air plant poutine. Coloring book hammock raclette glossier umami yr, truffaut knausgaard forage bespoke mustache.",
                        Author = user1,
                        Category = Category.World,
                        Tags = "world, news, politics, hope, optimism",
                        Comments = new List<Comment>()
                        {
                            new Comment() 
                            {  
                                Content = "Kogi scenester iceland neutra polaroid tumeric snackwave craft beer. Authentic wolf man bun succulents messenger bag blog. Hexagon hoodie schlitz, celiac migas trust fund whatever.", 
                                User = user1,
                            },
                            new Comment() 
                            {  
                                Content = "Street art godard viral photo booth succulents hexagon. Occupy tumeric twee biodiesel, ", 
                                User = user3,
                            }
                        }
                    },
                    new Thought()
                    {
                        Title = "The World Actually Is All Bad",
                        Content = "Single-origin coffee pabst succulents hexagon. Readymade try-hard prism semiotics. Art party YOLO meggings pitchfork cliche, jianbing scenester artisan cornhole green juice air plant retro sustainable. Mumblecore kinfolk chia, plaid stumptown disrupt tattooed irony migas retro gentrify flannel neutra hell of fixie. Schlitz enamel pin air plant PBR&B, migas organic everyday carry literally dreamcatcher kogi lo-fi chambray hammock tumeric. Waistcoat humblebrag prism, street art fap etsy meh knausgaard crucifix small batch bushwick butcher DIY poutine salvia. Synth etsy sriracha, chillwave authentic meggings gastropub retro artisan brunch.",
                        Author = user2,
                        Category = Category.World,
                        Tags = "depression, doomsday",
                        Comments = new List<Comment>()
                        {
                            new Comment() 
                            {  
                                Content = "Kogi scenester iceland neutra polaroid tumeric snackwave craft beer. Authentic wolf man bun succulents messenger bag blog. Hexagon hoodie schlitz, celiac migas trust fund whatever.", 
                                User = user1,
                            },
                            new Comment() 
                            {  
                                Content = "Street art godard viral photo booth succulents hexagon. Occupy tumeric twee biodiesel, ", 
                                User = user3,
                            }
                        }
                    },
                    new Thought()
                    {
                        Title = "This is a Very Personal Confession",
                        Content = "Stumptown iceland lumbersexual vexillologist put a bird on it.\n\n Street art organic butcher, cliche lumbersexual keffiyeh wolf neutra tumeric biodiesel asymmetrical celiac swag. Poke austin umami, pok pok meggings church-key lomo. Health goth hella pitchfork craft beer listicle celiac.",
                        Tags = "personal, confession",
                        Author = user2,
                        DisableComments = true,
                        Comments = new List<Comment>()
                    },
                    new Thought()
                    {
                        Title = "This is a ANOTHER Very Personal Confession",
                        Content = "Stumptown iceland lumbersexual vexillologist put a bird on it.\n\n Street art organic butcher, cliche lumbersexual keffiyeh wolf neutra tumeric biodiesel asymmetrical celiac swag. Poke austin umami, pok pok meggings church-key lomo. Health goth hella pitchfork craft beer listicle celiac.",
                        Tags = "personal, confession",
                        Author = user3,
                        Comments = new List<Comment>()
                    }                        
                };

                foreach (var thought in Thoughts)
                {
                    context.Thoughts.Add(thought);
                    context.Comments.AddRange(thought.Comments);
                }

                await context.SaveChangesAsync();
            }
        }

        public async static Task<User> SeedUserDataAsync(UserManager<User> userManager, 
            string userName, string email, string password, string first, string last, 
            string avatar)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var newUser = new User
                {
                    UserName = userName,
                    Email = email,
                    FirstName = first,
                    LastName = last,
                    Avatar = avatar
                };

                var result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded) 
                {
                    Console.WriteLine($"User {newUser} successfully created");
                }
                else 
                {
                    Console.WriteLine($"Errors occured trying to creat {newUser}");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }

                return newUser;
            }

            return user;
        }
    }
}