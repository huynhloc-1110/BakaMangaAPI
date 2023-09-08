using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public partial class SeedData
{
    private void SeedMangas(
        Dictionary<string, Category> categories,
        Dictionary<string, Author> authors,
        Dictionary<string, Group> groups
    )
    {
        var coverBaseUrl = _configuration["JWT:ValidIssuer"] + "/img/covers/";
        var pageUrl = _configuration["JWT:ValidIssuer"] + "/img/pages/";
        List<Manga> mangas = new();

        var uploader1 = _userManager.FindByEmailAsync("LocLe345@example.com").Result;
        var uploader2 = _userManager.FindByEmailAsync("Admin1@example.com").Result;
        var uploader3 = _userManager.FindByEmailAsync("Uploader1@example.com").Result;

        mangas.Add(
            new()
            {
                OriginalTitle = "Doraemon",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "Doraemon; ドラえもん; 도라에몽; 机器猫; Đôrêmon",
                Description =
                    "\"Doraemon\" is the masterpiece of Fujiko F. Fujio, one of Japan's most famous mangaka duos. Doraemon, a cat-shaped robot from the future, and his best friend Nobita share a fantastical friendship. The mysterious tools taken out of his 4th-dimensional pocket made the whole of Japan laugh. Shizu-chan, Suneo, and Gian are also full of energy. The exciting and wonderful tools that give you big dreams will guide you to the heartwarming world of Doraemon.",
                PublishYear = 1969,
                Categories = new List<Category>()
                {
                    categories["Adventure"],
                    categories["Comedy"],
                    categories["Fantasy"],
                    categories["Sci-Fi"],
                    categories["Slice of Life"],
                },
                Authors = new List<Author>() { authors["Fujiko F. Fujio"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter()
                    {
                        Number = 1,
                        Name = "All the Way From a Future World",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                        ChapterViews = new()
                        {
                            new() { User = uploader1 },
                            new() { User = uploader2 },
                            new() { User = uploader3 },
                        },
                        Pages = new()
                        {
                            new () { Number = 1, Path = pageUrl + "doraemon-chapter-1.1.png" },
                            new () { Number = 2, Path = pageUrl + "doraemon-chapter-1.2.png" },
                            new () { Number = 3, Path = pageUrl + "doraemon-chapter-1.3.png" }
                        }
                    },
                    new Chapter()
                    {
                        Number = 1,
                        Name = "Mirai no Kuni Kara Harubaruto",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                        UploadingGroup = groups["Japan Group"],
                    },
                    new Chapter()
                    {
                        Number = 1,
                        Name = "Người bạn đến từ tương lai",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "The Prophecy of Doraemon",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                        Pages = new()
                        {
                            new () { Number = 1, Path = pageUrl + "doraemon-chapter-2.1.png" },
                            new () { Number = 2, Path = pageUrl + "doraemon-chapter-2.2.png" },
                            new () { Number = 3, Path = pageUrl + "doraemon-chapter-2.3.png" }
                        }
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "Doraemon no Daiyogen",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                        UploadingGroup = groups["Japan Group"],
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "Lời tiên tri của Doraemon",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Transforming Biscuits",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                        Pages = new()
                        {
                            new () { Number = 1, Path = pageUrl + "doraemon-chapter-3.1.png" },
                            new () { Number = 2, Path = pageUrl + "doraemon-chapter-3.2.png" },
                            new () { Number = 3, Path = pageUrl + "doraemon-chapter-3.3.png" }
                        }
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Henshin Bisuketto",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                        UploadingGroup = groups["Japan Group"],
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Bánh quy biến hình",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                    new Chapter()
                    {
                        Number = 4,
                        Name = "Secret Agent",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                        Pages = new()
                        {
                            new () { Number = 1, Path = pageUrl + "doraemon-chapter-4.1.png" },
                            new () { Number = 2, Path = pageUrl + "doraemon-chapter-4.2.png" },
                            new () { Number = 3, Path = pageUrl + "doraemon-chapter-4.3.png" }
                        }
                    },
                    new Chapter()
                    {
                        Number = 4,
                        Name = "Hi (maru kakomi) supaidaisakusen",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                        UploadingGroup = groups["Japan Group"],
                    },
                    new Chapter()
                    {
                        Number = 4,
                        Name = "Chiến dịch do thám tuyệt mật",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                    new Chapter()
                    {
                        Number = 5,
                        Name = "Test chap 5",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                    new Chapter()
                    {
                        Number = 5,
                        Name = "Kobe Abe",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                        Pages = new()
                        {
                            new () { Number = 1, Path = pageUrl + "doraemon-chapter-5.1.png" },
                            new () { Number = 2, Path = pageUrl + "doraemon-chapter-5.2.png" },
                            new () { Number = 3, Path = pageUrl + "doraemon-chapter-5.3.png" }
                        }
                    },
                    new Chapter()
                    {
                        Number = 5,
                        Name = "Kobeabe",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                        UploadingGroup = groups["Japan Group"],
                    },
                    new Chapter()
                    {
                        Number = 5,
                        Name = "Ống sáo ngược đời",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                    new Chapter()
                    {
                        Number = 6,
                        Name = "Test Chap 6",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter()
                    {
                        Number = 6.5f,
                        Name = "Test Chap 6.5",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                    new Chapter()
                    {
                        Number = 8,
                        Name = "Test Chap 8",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["VietNam Group"],
                    },
                },
                CoverPath = coverBaseUrl + "doraemon.png", 
                Ratings = new()
                {
                    new() { Value = 5, User = uploader1 },
                    new() { Value = 4, User = uploader2 },
                    new() { Value = 4, User = uploader3 },
                },
                Followers = new() { uploader1, uploader2, uploader3 },
                Comments = new()
                {
                    new()
                    {
                        Content =
                            "Everytimes I read this chapter I really want to cry, I wish that I have this good parents.",
                        User = uploader1,
                        ChildComments = new()
                        {
                            new()
                            {
                                Content = "Don't be so deep bro, it's just a manga :))",
                                User = uploader2,
                                ChildComments = new()
                                {
                                    new() { Content = "Sssh, Let him cook.", User = uploader3 }
                                },
                                Reacts = new()
                                {
                                    new() { ReactFlag = ReactFlag.Dislike, User = uploader1 },
                                    new() { ReactFlag = ReactFlag.Like, User = uploader2 },
                                    new() { ReactFlag = ReactFlag.Dislike, User = uploader3 }
                                }
                            },
                            new() { Content = "Me too bro :<", User = uploader3 },
                            new() { Content = "Me three bro :<", User = uploader1 },
                            new() { Content = "+1 ", User = uploader2 }
                        },
                        Reacts = new()
                        {
                            new() { ReactFlag = ReactFlag.Like, User = uploader1 },
                            new() { ReactFlag = ReactFlag.Like, User = uploader2 },
                            new() { ReactFlag = ReactFlag.Dislike, User = uploader3 }
                        }
                    },
                    new()
                    {
                        Content =
                            "At least you got married. I may be still a child I have higher chances of being alone ;v; even though I'm practically better than you in every category except being male",
                        User = uploader3
                    },
                    new()
                    {
                        Content = "Thank you for scanlating this, whichever group you are",
                        User = uploader2
                    },
                    new()
                    {
                        Content =
                            "Doraemon was my first anime I ever watched and has stuck to me. For years to come I will never forget it. I hope your childhood has been great and have a wonderful years to come. Well with that I sign out.",
                        User = uploader1
                    },
                },
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Naruto",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "Naruto; ナルト; 나루토;狐忍",
                Description =
                    "Before Naruto's birth, a fantastic demon fox had attacked the Hidden Leaf Village. The 4th Hokage from the leaf village sealed the demon inside the newly born Naruto, causing him to unknowingly grow up detested by his fellow villagers. Despite his lack of talent in many areas of ninjutsu, Naruto strives for only one goal: to gain the title of Hokage, the strongest ninja in his village. Desiring the respect he never received, Naruto works toward his dream with fellow friends Sasuke and Sakura and mentor Kakashi as they go through many trials and battles that come with being a ninja.",
                PublishYear = 1999,
                Categories = new List<Category>()
                {
                    categories["Action"],
                    categories["Adventure"],
                    categories["Comedy"],
                    categories["Drama"],
                    categories["Fantasy"],
                    categories["Shounen"],
                },
                Authors = new List<Author>() { authors["Masashi Kishimoto"], },
                Chapters = new List<Chapter>
                {
                    new Chapter()
                    {
                        Number = 1,
                        Name = "Naruto Uzumaki!!",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "Konohamaru!!",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Sasuke Uchiha!!",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "naruto.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Komi-san wa Komyushou Desu",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles =
                    "Komi-san Can't Communicate; 古見さんは、コミュ症です。; 코미 양은, 커뮤증이에요;古见同学有交流障碍症; Komi-san không thể giao tiếp",
                Description =
                    "Komi-san is a beautiful and admirable girl whom no one can take their eyes off. Almost the whole school sees her as a cold beauty that's out of their league, but Tadano Hitohito knows the truth: she's just really bad at communicating with others. Komi-san, who wishes to fix this bad habit of hers, tries to improve it with the help of Tadano-kun by achieving her goal of having 100 friends.",
                PublishYear = 2016,
                Categories = new List<Category>()
                {
                    categories["Comedy"],
                    categories["Romance"],
                    categories["School Life"],
                    categories["Shoujo"],
                },
                Authors = new List<Author>() { authors["Oda Tomohito"], },
                Chapters = new List<Chapter>
                {
                    new Chapter()
                    {
                        Number = 1,
                        Name = "A normal person",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "Peaceful",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Spectre",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "komi.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "The Legendary Moonlight Sculptor",
                OriginalLanguage = Language.Korean,
                AlternativeTitles =
                    "The Legendary Moonlight Sculptor; 月光彫刻師; 달빛 조각사, 커뮤증이에요; 月光彫刻師; Hành trình đế vương",
                Description =
                    "The man forsaken by the world, the man a slave to money, and the man known as the legendary God of War in the highly popular MMORPG Continent of Magic. With the coming of age, he decides to say goodbye, but the feeble attempt to earn a little something for his time and effort ripples into an effect none could ever have imagined. Through a series of coincidences, his legendary avatar is sold for 3 billion 90 million won ($2.7 million), bringing great joy to him, only to plunge him into despair at losing almost all of it to vicious loan sharks. With the revelation of money through gaming, he rises from the abyss with newfound resolve and steps forward into the new age of games led by the first-ever Virtual Reality MMORPG, Royal Road.",
                PublishYear = 2015,
                Categories = new List<Category>()
                {
                    categories["Action"],
                    categories["Adventure"],
                    categories["Fantasy"],
                    categories["Game"],
                    categories["Martial Arts"],
                    categories["Sci-Fi"],
                    categories["Seinen"],
                },
                Authors = new List<Author>() { authors["Heesung NAM"], },
                Chapters = new List<Chapter>
                {
                    new Chapter()
                    {
                        Number = 1,
                        Name = "Introduction: The Legendary Moonlight Sculptor",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "The Man with a Mask",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "The Master and Disciple",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "the.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Itou Junji Kyoufu Manga Collection",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "伊藤润二恐怖漫画精选; Ryokan; 이토 준지 컬렉션; 학교괴담",
                Description =
                    "The first two volumes contain thematically linked but self-contained stories concerning Tomie - a beautiful young woman with the power to seduce and dominate any male, from small boys to elderly men. Later, the series leaves these characters behind and focuses on stand-alone gothic horror pieces.",
                PublishYear = 1989,
                Categories = new List<Category>()
                {
                    categories["Comedy"],
                    categories["Drama"],
                    categories["Horror"],
                    categories["Mystery"],
                    categories["Supernatural"],
                    categories["Tragedy"],
                    categories["Psychological"],
                },
                Authors = new List<Author>() { authors["Itou Junji"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter
                    {
                        Number = 1,
                        Name = "Tomie",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "Photograph",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "Kiss",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "itou.png",     
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Heroine Hajimemashita",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "Heroine for Hire; 今天开始成为女主角",
                Description =
                    "Shuko Kodakamine is strong—too strong, by the standards of some. She hopes to make a good high school debut, but that plan goes up in smoke almost immediately when she injures the attractive, charismatic Serizawa...and later suplexes him, to boot! But when Serizawa finds himself in hot water with a jealous boyfriend, it's Shuko who comes to his aid...and he comes up with a ridiculous proposition: If Shuko becomes his bodyguard, he'll make her the most important girl in his world!",
                PublishYear = 2018,
                Categories = new List<Category>()
                {
                    categories["Comedy"],
                    categories["Romance"],
                    categories["School Life"],
                    categories["Shoujo"],
                },
                Authors = new List<Author>() { authors["Fuyu Amakura"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter
                    {
                        Number = 1,
                        Name = "Encounter With A High School Student",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "A great success!!",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "I became his bodyguard",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "heroine.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Tsui no Taimashi -Endaa Gaisutaa-",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "The Last Exorcist -Ender Geister-",
                Description =
                    "This protagonist is the most demonically strong exorcist. Akira, the S-class exorcist, sent from Germany to investigate a paranormal event in a certain city in Japan. He shoots creatures from other worlds with his beautiful exorcist partner Chikage.",
                PublishYear = 2019,
                Categories = new List<Category>()
                {
                    categories["Action"],
                    categories["Drama"],
                    categories["Mature"],
                    categories["Mystery"],
                    categories["Shounen"],
                    categories["Supernatural"],
                },
                Authors = new List<Author>() { authors["YOMOYAMA Takashi"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter
                    {
                        Number = 1,
                        Name = "August eight",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "Con air",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "Eastern Promises",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "tsui.jpg",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Meitantei Conan",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "Case Closed; 명탐정 코난; 名侦探柯南; Thám Tử Lừng Danh Conan",
                Description =
                    "Shinichi Kudo is a high school detective who sometimes works with the police to solve cases. During an investigation, he is attacked by members of a crime syndicate known as the Black Organization. They force him to ingest an experimental poison, but instead of killing him, the poison transforms him into a child. Adopting the pseudonym Conan Edogawa and keeping his true identity a secret, Kudo lives with his childhood friend Ran and her father Kogoro, who is a private detective.",
                PublishYear = 1994,
                Categories = new List<Category>()
                {
                    categories["Action"],
                    categories["Adventure"],
                    categories["Comedy"],
                    categories["Detective"],
                    categories["Drama"],
                    categories["Mystery"],
                    categories["Shounen"],
                },
                Authors = new List<Author>() { authors["Gosho Aoyama"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter
                    {
                        Number = 1,
                        Name = "The Heisei Holmes",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "The shrunken detective",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "The left out detective",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "conan.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Yaiteru Futari",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "A Rare Marriage: How to Grill Our Love; 굽고 있는 두 사람; 爱情是烤肉的滋味!",
                Description =
                    "After their fateful encounter through a dating app, Kenta and Chihiro decided to get married with zero days of dating in light of Kenta's transfer. As a couple who got married without knowing much about each other, they deepened their relationship through their regular weekend BBQs.",
                PublishYear = 2020,
                Categories = new List<Category>()
                {
                    categories["Cooking"],
                    categories["Romance"],
                    categories["Slice of Life"],
                },
                Authors = new List<Author>() { authors["Hanatsuka Shiori"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter
                    {
                        Number = 1,
                        Name = "You apply just the right amount of heat",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "First breakfast together",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "A Toast with Beer and lamp chops",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "yaiteru.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "One Piece",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "원피스;  海贼王; Đảo Hải Tặc",
                Description =
                    "Gol D. Roger, a man referred to as the \"Pirate King,\" is set to be executed by the World Government. But just before his demise, he confirms the existence of a great treasure, One Piece, located somewhere within the vast ocean known as the Grand Line. Announcing that One Piece can be claimed by anyone worthy enough to reach it, the Pirate King is executed, and the Great Age of Pirates begins.",
                PublishYear = 1997,
                Categories = new List<Category>()
                {
                    categories["Action"],
                    categories["Comedy"],
                    categories["Fantasy"],
                },
                Authors = new List<Author>() { authors["Oda Eiichiro"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter
                    {
                        Number = 1,
                        Name = "Romance Dawn",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "They Call Him “Straw Hat Luffy",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "Enter Zolo: Pirate Hunter",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "one.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Black Clover",
                OriginalLanguage = Language.Japanese,
                AlternativeTitles = "블랙 클로버; 黑色五叶草",
                Description =
                    "Asta and Yuno were abandoned together at the same church, and have been inseparable since. As children, they promised that they would compete against each other to see who would become the next Emperor Magus. However, as they grew up, some differences between them became plain. When they received their Grimoires at age 15, Yuno got a spectacular book with a four-leaf clover (most people receive a three-leaf clover), while Asta received nothing at all.",
                PublishYear = 2015,
                Categories = new List<Category>()
                {
                    categories["Action"],
                    categories["Adventure"],
                    categories["Military"],
                },
                Authors = new List<Author>() { authors["Tabata Yuuki"], },
                Chapters = new List<Chapter>()
                {
                    new Chapter
                    {
                        Number = 1,
                        Name = "The Boy’s Vow",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "Magic Knights Entrance Exam",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "The Road to the Magic Emperor",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
                },
                CoverPath = coverBaseUrl + "black.png",
            }
        );

        mangas.Add(
           new()
           {
               OriginalTitle = "Drifters",
               OriginalLanguage = Language.Japanese,
               AlternativeTitles = "ドリフターズ; 漂泊者",
               Description =
                   "An isekai series from the creator of Hellsing. The story of Drifters begins during Japan's Sengoku Period (~1467-1600), a time of constant warring between the island's states. A samurai on the verge of death suddenly finds himself thrown into another, fantastic world, alongside important military figures from history. It's time to battle in a brand-new world war.\r\n",
               PublishYear = 2009,
               Categories = new List<Category>()
               {
                    categories["Historical"],
                    categories["Isekai"],
                    categories["Comedy"],
               },
               Authors = new List<Author>() { authors["Hirano Kouta"], },
               Chapters = new List<Chapter>()
               {
                    new Chapter
                    {
                        Number = 1,
                        Name = "Fight Song",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "Transcending Times",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "The Devil",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
               },
               CoverPath = coverBaseUrl + "drifters.png",
           }
       );

        mangas.Add(
           new()
           {
               OriginalTitle = "Bleach",
               OriginalLanguage = Language.Japanese,
               AlternativeTitles = "Sứ Mạng Thần Chết; 블리치; 死神",
               Description =
                   "Ichigo Kurosaki has always been able to see ghosts, but this ability doesn't change his life nearly as much as his close encounter with Rukia Kuchiki, a Soul Reaper and member of the mysterious Soul Society",
               PublishYear = 2001,
               Categories = new List<Category>()
               {
                    categories["Action"],
                    categories["Adventure"],
                    categories["Comedy"],
               },
               Authors = new List<Author>() { authors["Hirano Kouta"], },
               Chapters = new List<Chapter>()
               {
                    new Chapter
                    {
                        Number = 1,
                        Name = "Strawberry & the Soul Reapers",
                        Language = Language.English,
                        Uploader = uploader1,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 2,
                        Name = "Starter",
                        Language = Language.English,
                        Uploader = uploader2,
                        UploadingGroup = groups["English Group"],
                    },
                    new Chapter
                    {
                        Number = 3,
                        Name = "Head-Hittin'",
                        Language = Language.English,
                        Uploader = uploader3,
                        UploadingGroup = groups["English Group"],
                    },
               },
               CoverPath = coverBaseUrl + "bleach.png",
           }
       );

        _context.AddRange(mangas);
        _context.SaveChanges();
    }
}
