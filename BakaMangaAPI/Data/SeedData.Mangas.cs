﻿using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public partial class SeedData
{
    private void SeedMangas(
        Dictionary<string, Category> categories,
        Dictionary<string, Author> authors
    )
    {
        var serverUrl = _configuration["JWT:ValidIssuer"] + "/img/";
        List<Manga> mangas = new();

        var uploader1 = _userManager.FindByEmailAsync("LocLe345@example.com").Result;
        var uploader2 = _userManager.FindByEmailAsync("TriTat567@example.com").Result;
        var uploader3 = _userManager.FindByEmailAsync("HuyNguyen123@example.com").Result;

        mangas.Add(
            new()
            {
                OriginalTitle = "Doraemon (ドラえもん)",
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
                    },
                    new Chapter()
                    {
                        Number = 1,
                        Name = "Mirai no Kuni Kara Harubaruto",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                    },
                    new Chapter()
                    {
                        Number = 1,
                        Name = "Người bạn đến từ tương lai",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "The Prophecy of Doraemon",
                        Language = Language.English,
                        Uploader = uploader1,
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "Doraemon no Daiyogen",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                    },
                    new Chapter()
                    {
                        Number = 2,
                        Name = "Lời tiên tri của Doraemon",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Transforming Biscuits",
                        Language = Language.English,
                        Uploader = uploader1,
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Henshin Bisuketto",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                    },
                    new Chapter()
                    {
                        Number = 3,
                        Name = "Bánh quy biến hình",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                    },
                    new Chapter()
                    {
                        Number = 4,
                        Name = "Secret Agent",
                        Language = Language.English,
                        Uploader = uploader1,
                    },
                    new Chapter()
                    {
                        Number = 4,
                        Name = "Hi (maru kakomi) supaidaisakusen",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                    },
                    new Chapter()
                    {
                        Number = 4,
                        Name = "Chiến dịch do thám tuyệt mật",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                    },
                    new Chapter()
                    {
                        Number = 5,
                        Name = "Kobe Abe",
                        Language = Language.English,
                        Uploader = uploader1,
                    },
                    new Chapter()
                    {
                        Number = 5,
                        Name = "Kobeabe",
                        Language = Language.Japanese,
                        Uploader = uploader2,
                    },
                    new Chapter()
                    {
                        Number = 5,
                        Name = "Ống sáo ngược đời",
                        Language = Language.Vietnamese,
                        Uploader = uploader3,
                    },
                },
                CoverPath = serverUrl + "doraemon.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Naruto (ナルト)",
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
                         Uploader = uploader1
                     },
                     new Chapter()
                     {
                         Number = 2,
                         Name = "Konohamaru!!",
                         Language = Language.English,
                         Uploader = uploader2
                     },
                     new Chapter()
                     {
                         Number = 3,
                         Name = "Sasuke Uchiha!!",
                         Language = Language.English,
                         Uploader = uploader3
                     },
                },
                CoverPath = serverUrl + "naruto.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Komi-san wa Komyushou Desu (古見さんは、コミュ症です。)",
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
                         Uploader = uploader1
                     },
                     new Chapter() 
                     { 
                         Number = 2, 
                         Name = "Peaceful", 
                         Language = Language.English, 
                         Uploader = uploader2 
                     },
                     new Chapter() 
                     { 
                         Number = 3, 
                         Name = "Spectre", 
                         Language = Language.English, 
                         Uploader = uploader3 
                     },
                },
                CoverPath = serverUrl + "komi.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "The Legendary Moonlight Sculptor (달빛 조각사)",
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
                         Uploader = uploader1
                     },
                     new Chapter() 
                     { 
                         Number = 2, 
                         Name = "The Man with a Mask", 
                         Language = Language.English, 
                         Uploader = uploader2 
                     },
                     new Chapter() 
                     { 
                         Number = 3, 
                         Name = "The Master and Disciple", 
                         Language = Language.English, 
                         Uploader = uploader3 
                     },
                },
                CoverPath = serverUrl + "the.png",
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
                         Uploader = uploader1 
                     },
                     new Chapter 
                     { 
                         Number = 2, 
                         Name = "Photograph", 
                         Language = Language.English, 
                         Uploader = uploader2 
                     },
                     new Chapter 
                     { 
                         Number = 3, 
                         Name = "Kiss", 
                         Language = Language.English, 
                         Uploader = uploader3 
                     },
                },
                CoverPath = serverUrl + "itou.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Heroine Hajimemashita (ヒロインはじめました。)",
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
                         Uploader = uploader1 
                     },
                     new Chapter 
                     { 
                         Number = 2, 
                         Name = "A great success!!", 
                         Language = Language.English, 
                         Uploader = uploader2 
                     },
                     new Chapter 
                     { 
                         Number = 3, 
                         Name = "I became his bodyguard", 
                         Language = Language.English, 
                         Uploader = uploader3 
                     },
                },
                CoverPath = serverUrl + "heroine.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Tsui no Taimashi -Endaa Gaisutaa- (終の退魔師―エンダーガイスター)",
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
                         Uploader = uploader1 
                     },
                     new Chapter 
                     { 
                         Number = 2, 
                         Name = "Con air", 
                         Language = Language.English, 
                         Uploader = uploader2 
                     },
                     new Chapter 
                     { 
                         Number = 3, 
                         Name = "Eastern Promises", 
                         Language = Language.English, 
                         Uploader = uploader3 
                     },
                },
                CoverPath = serverUrl + "tsui.jpg",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Meitantei Conan (名探偵コナン)",
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
                         Uploader = uploader1 
                     },
                     new Chapter 
                     { 
                         Number = 2, 
                         Name = "The shrunken detective", 
                         Language = Language.English, 
                         Uploader = uploader2 
                     },
                     new Chapter 
                     { 
                         Number = 3, 
                         Name = "The left out detective", 
                         Language = Language.English, 
                         Uploader = uploader3 
                     },
                },
                CoverPath = serverUrl + "conan.png",
            }
        );

        mangas.Add(
            new()
            {
                OriginalTitle = "Yaiteru Futari (焼いてるふたり)",
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
                         Uploader = uploader1 
                     },
                     new Chapter 
                     { 
                         Number = 2, 
                         Name = "First breakfast together", 
                         Language = Language.English, 
                         Uploader = uploader2 
                     },
                     new Chapter 
                     { 
                         Number = 3, 
                         Name = "A Toast with Beer and lamp chops", 
                         Language = Language.English, 
                         Uploader = uploader3 
                     },
                },
                CoverPath = serverUrl + "yaiteru.png",
            }
        );

        _context.AddRange(mangas);
        _context.SaveChanges();
    }
}
