using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BakaMangaAPI.Data;

public class SeedData : IDisposable
{
    private readonly ApplicationDbContext _context;
    public SeedData(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    public void Initialize()
    {
        if (_context.Mangas.Any())
        {
            return;
        }


        // CATEGORY SEED DATA
        Category action = new() {
            Name = "Action",
            Description = "The action genre is a class of creative works characterized by more emphasis on exciting action sequences than on character development or story-telling. It is usually possible to tell from the creative style of an action sequence, the genre of the entire creative work. For example, the style of a combat sequence will indicate whether the entire work is an action-adventure or martial art."
        };

        Category adventure = new()
        {
            Name = "Adventure",
            Description = "The focus is on the hero's literal journey towards some goal and all the wacky stuff that happens along the way. These animated works often feature fantastical, imaginative worlds and intricate storylines that explore themes such as love, adventure, friendship, and personal growth."
        };

        Category comedy = new()
        {
            Name = "Comedy",
            Description = "A genre that focuses on humor and laughter."
        };

        Category cooking = new()
        {
            Name = "Cooking",
            Description = "A genre of anime that focuses on food and cooking, showcasing the daily lives of those who work in the food industry and highlighting various aspects of food appreciation and presentation. These anime shows feature characters who are passionate about cooking and baking, and they explore different cooking techniques, recipes, and culinary cultures from around the world."
        };

        Category detective = new()
        {
            Name = "Detective",
            Description = "A genre that revolves around investigations, crime-solving, and detectives. This genre typically features a detective or a group of detectives who solve mysteries and crimes through their intelligence and skills."
        };

        Category drama = new()
        {
            Name = "Drama",
            Description = "Focuses on serious and emotional themes, often portraying characters dealing with difficult situations or overcoming personal struggles."
        };

        Category fantasy = new()
        {
            Name = "Fantasy",
            Description = "A genre that typically involves magical elements and is often set in a fictional universe. This also revolves around magic and supernatural events, featuring magical creatures and settings that may not be possible in reality."
        };

        Category game = new()
        {
            Name = "Game",
            Description = "There are various game categories in anime, including anime-based games and game anime. Anime-based games are computer and video games that are based on manga or anime properties, while game anime showcases people's enthusiasm and love for gaming, regardless of the type of game. Game anime often shows characters growing and developing deeper strategies, building their confidence as players, and mastering moves that once seemed impossible."
        };

        Category historical = new()
        {
            Name = "Historical",
            Description = "A genre that is set in a past time period and often features historical events, people, or places. These anime can be either serious or comedic, and they can take place in different parts of the world."
        };

        Category horror = new()
        {
            Name = "Horror",
            Description = "A genre that features elements of fear, terror, and the macabre. Horror can include supernatural occurrences, monsters, ghosts, and serial killers."
        };

        Category isekai = new()
        {
            Name = "Isekai",
            Description = "A genre that features displaced protagonists who are transported to and need to survive in another world, such as a fantasy world, virtual world, or parallel universe. This genre can be divided into different types based on the plot and storylines."
        };

        Category josei = new()
        {
            Name = "Josei",
            Description = "A genre of anime and manga that is targeted toward adult women. It deals with realistic and complex themes such as work-life balance, relationships, and social issues in a mature and nuanced way."
        };

        Category martialArts = new()
        {
            Name = "Martial Arts",
            Description = "Martial arts series often showcase the unique fighting styles of different characters and blend real-world techniques with supernatural abilities and excellent choreography. Some series use martial arts in unique ways to create character-driven stories, while others immerse the viewer in a world of fighters and warriors."
        };

        Category mature = new()
        {
            Name = "Mature",
            Description = "A genre that contains content that is not suitable for younger audiences. This includes mature themes such as violence, sexual content, and profanity. Mature series often explore complex and thought-provoking topics, such as politics, psychology, and philosophy. These mangas are typically targeted towards an older demographic and feature more realistic and nuanced portrayals of characters and situations."
        };

        Category mecha = new()
        {
            Name = "Mecha",
            Description = "Focuses on giant robots or mechs, typically piloted by humans, engaged in battles or other types of action. Mecha stories often incorporate elements of science fiction, action, adventure, and drama, and can also feature political or philosophical themes."
        };

        Category military = new()
        {
            Name = "Military",
            Description = "Stories that revolve around military organizations, personnel, and warfare. These stories often feature historical or futuristic settings and explore themes of patriotism, honor, sacrifice, and the human cost of war."
        };

        Category mystery = new()
        {
            Name = "Mystery",
            Description = "Stories that involve a puzzle or a problem that needs to be solved. The plot typically centers around a crime or a series of crimes that must be deciphered by the protagonist or a group of characters. Mysteries can range from simple whodunits to complex stories that require the reader to solve the puzzle along with the characters. The genre often features twists and turns, red herrings, and unexpected revelations that keep the reader guessing until the very end."
        };

        Category oneshot = new()
        {
            Name = "Oneshot",
            Description = "A standalone story that is published in a single chapter or issue. These stories are self-contained, with a complete plot, characters, and setting, and are not part of a larger series or ongoing story. Oneshots can be of any genre and can vary in length, from a few pages to several chapters. They are often used by manga creators as a way to experiment with new ideas, test the waters with readers, or showcase their skills."
        };

        Category psychological = new()
        {
            Name = "Psychological",
            Description = "Focus on the internal struggles and mental states of the characters. These stories often explore complex themes such as trauma, mental illness, and the human psyche."
        };

        Category romance = new()
        {
            Name = "Romance",
            Description = "Focus on characters who are navigating their feelings and emotions, as well as the feelings of those around them. This makes for a relatable story that is both entertaining and thought-provoking. In addition to exploring love and relationships, Romance manga also explores themes such as family, friendship, self-discovery, and growth. These stories can be very inspiring to readers of all ages, as they demonstrate how characters can overcome obstacles to find happiness."
        };

        Category schoolLife = new()
        {
            Name = "School Life",
            Description = "Focus on the experiences of characters in a high school setting. This genre typically explores themes such as self-discovery, personal image, friendships, and romance. The manga in this category showcase events that occur on a daily basis in a school, whether from the perspective of a student or of a teacher."
        };

        Category sciFi = new()
        {
            Name = "Sci-Fi",
            Description = "A category that explores new worlds, either in the future or past, on Earth or other planets. It features humanity's exploration of new technologies, societies, or frontiers in outer space. This category is based on imagination and creativity and often involves imagined technological advancements or natural settings."
        };

        Category seinen = new()
        {
            Name = "Seinen",
            Description = "A category that is marketed towards adult men between the ages of 18 and 40. It covers a wide range of genres such as action, politics, science fiction, fantasy, relationships, sports, or comedy. It is often more sophisticated, psychological, satirical, violent, sexual, and for a more mature audience."
        };

        Category shoujo = new()
        {
            Name = "Shoujo",
            Description = "A genre targeted towards young girls and women. It typically features romantic relationships, emotional drama, and character development. The stories often center around a female protagonist and explore themes such as love, friendship, and self-discovery. Shoujo mangas are known for their unique art style, which often includes large, expressive eyes and intricate detail work."
        };

        Category shounen = new()
        {
            Name = "Shounen",
            Description = "A category of manga marketed towards adolescent boys, characterized by high-action plots featuring male protagonists and emphasizing camaraderie among male characters. Shonen works often have more than a fair share of fanservice as well."
        };

        Category sliceOfLife = new()
        {
            Name = "Slice of Life",
            Description = "A genre that portrays everyday life, often focusing on the mundane and ordinary experiences of the characters. These stories can be humorous, heartwarming, or serious and often explore the relationships between characters as they navigate their daily lives. Slice of Life manga typically does not have a central plot or conflict and instead focuses on the characters' experiences and emotions."
        };

        Category sports = new()
        {
            Name = "Sports",
            Description = "A category in manga that features stories about athletes and their struggles to win in various sports competitions. This genre often focuses on themes of perseverance, teamwork, and overcoming obstacles."
        };

        Category supernatural = new()
        {
            Name = "Supernatural",
            Description = "Focuses on supernatural elements such as ghosts, vampires, demons, and other supernatural beings. The stories in this category often involve battles between good and evil and feature characters with supernatural abilities or powers."
        };

        Category thriller = new()
        {
            Name = "Thriller",
            Description = "A genre that focuses on suspense, excitement, and anticipation. The stories usually revolve around a central mystery or problem that the protagonist must solve. The genre is known for its twists and turns, unexpected plot developments, and fast-paced action."
        };

        Category tragedy = new()
        {
            Name = "Tragedy",
            Description = "A genre that deals with serious and often depressing subject matter. It typically involves the downfall or suffering of the protagonist or other characters and explores themes of loss, despair, and the fragility of human existence."
        };

    // AUTHOR SEED DATA
        Author kishimotoMasashi = new() {
            Name = "Masashi Kishimoto",
            Biography = "Masashi Kishimoto, born November 8, 1974, is a Japanese manga artist. His best-known work, Naruto, was in serialization from 1999 to 2014 and has sold over 250 million copies worldwide in 46 countries as of May 2019."
        };

        Author fujikoFujio = new()
        {
            Name = "Fujiko F. Fujio",
            Biography = "Fujiko Fujio was a pen name of a manga writing duo formed by two Japanese manga artists. Their real names are Hiroshi Fujimoto (1933-1996) and Motoo Abiko (1934-2022). They formed their partnership in 1951 and used the Fujiko Fujio name from 1954 until the dissolution of the partnership in 1987. From the outset, they adopted a collaborative style where both worked simultaneously on the story and artwork, but as they diverged creatively they started releasing individual works under different names, Abiko as Fujiko Fujio, and Fujimoto as Fujiko F. Fujio. Throughout their career, they won many individual and collaborative awards, and are best known for creating the popular and long-running series Doraemon, the main character of which is officially recognized as a cultural icon of modern Japan. Some influences of most of their projects are the works of acclaimed manga artist Osamu Tezuka and many US cartoons and comic books—including the works of Hanna-Barbera."
        };

        Author odaTomohito = new()
        {
            Name = "Oda Tomohito",
            Biography = "Oda Tomohito won the grand prize for World Worst One in the 70th Shogakukan New Comic Artist Awards in 2012. Oda's series Digicon, about a tough high school who finds herself in control of an alien with plans for world domination, ran from 2014 to 2015. In 2015, Komi-San Wa Komyushou Desu debuted as a one-shot in Weekly Shounen Sunday and was picked up as a full series by the same magazine in 2016."
        };

        Author heesungNam = new()
        {
            Name = "Heesung NAM",
            Biography = "Heesung NAM (October 5, 1980) is famously known as the author of The Legendary Moonlight Sculptor, a Korean game fantasy novel that has spanned 58 volumes since 2007."
        };

        Author itouJunji = new()
        {
            Name = "Itou Junji",
            Biography = "Junji Ito (born July 31, 1963) is a Japanese horror manga artist. Some of his most notable works include Tomie, a series chronicling an immortal girl who drives her stricken admirers to madness; Spiral Into Horror Uzumaki, a three-volume series about a town obsessed with spirals; and Gyo, a two-volume story where fish are controlled by a strain of sentient bacteria called 'the death stench.' His other works include Itou Junji Kyoufu Manga Collection, a collection of different short stories including a series of stories named Souichi's Journal of Delights, and Junji Ito's Cat Diary: Yon & Mu, a self-parody about him and his wife living in a house with two cats. Ito's work has developed a substantial cult following, with some deeming him a significant figure in recent horror iconography."
        };

        Author goshoAoyama = new() {
            Name = "Gosho Aoyama",
            Biography = "Gosho Aoyama (born June 21, 1963) is a Japanese manga artist best known for his manga series Detective Conan (1994–present), known as Case Closed in some English-speaking countries. As of 2017, his various manga series had a combined 200 million copies in print worldwide. Aoyama made his debut as a manga artist with Chotto Mattete, which was published in the magazine Weekly Shōnen Sunday in winter of 1987. [Shortly after that, he began Magic Kaito in the same magazine. Magic Kaito protagonist Kaito Kuroba later appeared in Case Closed. Between 1988 and 1993, Aoyama created the series Yaiba, which ran for 24 volumes. Later, he would release other manga series in single volumes, such as Third Baseman No.4 and Gosho Aoyama's Collection of Short Stories. Aoyama began serializing Detective Conan in Weekly Shōnen Sunday on January 19, 1994. When the series was first released in English, it was given the title Case Closed."
        };


        Author fuyuAmakura = new()
        {
            Name = "Fuyu Amakura",
            Biography = "You can find more about the author on their Twitter account: [Fuyu Amakura on Twitter](https://twitter.com/amakura_fuyu)"
        };

        Author yomoyamaTakashi = new()
        {
            Name = "YOMOYAMA Takashi",
            Biography = "YOMOYAMA Takashi's mother is from Germany. He attended a Japanese school in Germany up until Junior School before moving to Japan and continuing High School and University Education."
        };

        Author hanatsukaShiori = new()
        {
            Name = "Hanatsuka Shiori",
            Biography = "You can find more about the author on their Twitter account: [Hanatsuka Shiori on Twitter](https://twitter.com/hntkchan)"
        };

        // MANGA SEED DATA
        Manga doraemon = new()
        {
            OriginalTitle = "Doraemon (ドラえもん)",
            OriginalLanguage = Language.Japanese,
            AlternativeTitles = "Doraemon;ドラえもん;도라에몽;机器猫;Đôrêmon",
            Description = "\"Doraemon\" is the masterpiece of Fujiko F. Fujio, one of Japan's most famous mangaka duos. Doraemon, a cat-shaped robot from the future, and his best friend Nobita share a fantastical friendship. The mysterious tools taken out of his 4th-dimensional pocket made the whole of Japan laugh. Shizu-chan, Suneo, and Gian are also full of energy. The exciting and wonderful tools that give you big dreams will guide you to the heartwarming world of Doraemon.",
            PublishYear = 1969,
            Categories = new List<Category>() {
                adventure,
                comedy,
                fantasy,
                sciFi,
                sliceOfLife ,
            },
            Authors = new List<Author>() {
                fujikoFujio
            },
            Chapters = new List<Chapter>() {
                new Chapter() { Name = "Chapter 1: All the Way From a Future World", Language = Language.English},
                new Chapter() { Name = "Chapter 2: The Prophecy of Doraemon", Language = Language.English},
                new Chapter() { Name = "Chapter 3: Transforming Biscuits", Language = Language.English},
            }
        };

        Manga naruto = new()
        {
            OriginalTitle = "Naruto (ナルト)",
            OriginalLanguage = Language.Japanese,
            AlternativeTitles = "Naruto;ナルト;나루토;狐忍",
            Description = "Before Naruto's birth, a fantastic demon fox had attacked the Hidden Leaf Village. The 4th Hokage from the leaf village sealed the demon inside the newly born Naruto, causing him to unknowingly grow up detested by his fellow villagers. Despite his lack of talent in many areas of ninjutsu, Naruto strives for only one goal: to gain the title of Hokage, the strongest ninja in his village. Desiring the respect he never received, Naruto works toward his dream with fellow friends Sasuke and Sakura and mentor Kakashi as they go through many trials and battles that come with being a ninja.",
            PublishYear = 1999,
            Categories = new List<Category>() {
                action,
                adventure,
                comedy,
                drama,
                fantasy,
                shounen,
            },
            Authors = new List<Author>(){
                kishimotoMasashi
            },
            Chapters = new List<Chapter>{
                new Chapter() { Name = "Chapter 1: Naruto Uzumaki!!"},
                new Chapter() { Name = "Chapter 2: Konohamaru!!"},
                new Chapter() { Name = "Chapter 3: Sasuke Uchiha!!"},
            }
        };

        Manga Komi_san_wa_Komyushou_Desu = new()
        {
            OriginalTitle = "Komi-san wa Komyushou Desu (古見さんは、コミュ症です。)",
            OriginalLanguage = Language.Japanese,
            AlternativeTitles = "Komi-san Can't Communicate;古見さんは、コミュ症です。;코미 양은, 커뮤증이에요;古见同学有交流障碍症;Komi-san không thể giao tiếp",
            Description = "Komi-san is a beautiful and admirable girl whom no one can take their eyes off. Almost the whole school sees her as a cold beauty that's out of their league, but Tadano Hitohito knows the truth: she's just really bad at communicating with others. Komi-san, who wishes to fix this bad habit of hers, tries to improve it with the help of Tadano-kun by achieving her goal of having 100 friends.",
            PublishYear = 2016,
            Categories = new List<Category>() {
                comedy,
                romance,
                schoolLife,
                shoujo,
            },
            Authors = new List<Author>() {
                odaTomohito
            },
            Chapters = new List<Chapter>{
                new Chapter() { Name = "Chapter 1: A normal person"},
                new Chapter() { Name = "Chapter 2: Peaceful"},
                new Chapter() { Name = "Chapter 3: Spectre"},
            }
        };

        Manga The_Legendary_Moonlight_Sculptor = new()
        {
            OriginalTitle = "The Legendary Moonlight Sculptor (달빛 조각사)",
            OriginalLanguage = Language.Korean,
            AlternativeTitles = "The Legendary Moonlight Sculptor;月光彫刻師;달빛 조각사, 커뮤증이에요;月光彫刻師;Hành trình đế vương",
            Description = "The man forsaken by the world, the man a slave to money, and the man known as the legendary God of War in the highly popular MMORPG Continent of Magic. With the coming of age, he decides to say goodbye, but the feeble attempt to earn a little something for his time and effort ripples into an effect none could ever have imagined. Through a series of coincidences, his legendary avatar is sold for 3 billion 90 million won ($2.7 million), bringing great joy to him, only to plunge him into despair at losing almost all of it to vicious loan sharks. With the revelation of money through gaming, he rises from the abyss with newfound resolve and steps forward into the new age of games led by the first-ever Virtual Reality MMORPG, Royal Road. [This is the legend of Lee Hyun on his path to becoming Emperor with only his family-loving heart, his boundless desire for money, his unexpected mind, his diligently forged body, and the talent of hard work backing him. This is the legend of the lowest becoming the strongest. This is the legend of WEED.]",
            PublishYear = 2015,
            Categories = new List<Category>() {
                action,
                adventure,
                fantasy,
                game,
                martialArts,
                sciFi,
                seinen,
            },
            Authors = new List<Author>() {
                heesungNam
            },
            Chapters = new List<Chapter>{
                new Chapter() { Name = "Chapter 1: Introduction: The Legendary Moonlight Sculptor"},
                new Chapter() { Name = "Chapter 2: The Man with a Mask"},
                new Chapter() { Name = "Chapter 3: The Master and Disciple"},
            }
        };


        Manga itouJunjiCollection = new ()
        {
            OriginalTitle = "Itou Junji Kyoufu Manga Collection",
            OriginalLanguage = Language.Japanese,
            AlternativeTitles = "伊藤润二恐怖漫画精选; Ryokan; 이토 준지 컬렉션; 학교괴담",
            Description = "The first two volumes contain thematically linked but self-contained stories concerning Tomie - a beautiful young woman with the power to seduce and dominate any male, from small boys to elderly men. Later, the series leaves these characters behind and focuses on stand-alone gothic horror pieces.",
            PublishYear = 1989,
            Categories = new List<Category>()
            {
                comedy,
                drama,
                horror,
                mystery,
                supernatural,
                tragedy,
                psychological,
            },
            Authors = new List<Author>()
            {
                itouJunji
            },
            Chapters = new List<Chapter>()
            {
                new Chapter { Name = "Chapter 1: Tomie" },
                new Chapter { Name = "Chapter 2: Photograph" },
                new Chapter { Name = "Chapter 3: Kiss" }
            }
        };

        Manga heroineHajimemashita = new ()
        {
            OriginalTitle = "Heroine Hajimemashita (ヒロインはじめました。)",
            OriginalLanguage= Language.Japanese,
            AlternativeTitles = "Heroine for Hire; 今天开始成为女主角",
            Description = "Shuko Kodakamine is strong—too strong, by the standards of some. She hopes to make a good high school debut, but that plan goes up in smoke almost immediately when she injures the attractive, charismatic Serizawa...and later suplexes him, to boot! But when Serizawa finds himself in hot water with a jealous boyfriend, it's Shuko who comes to his aid...and he comes up with a ridiculous proposition: If Shuko becomes his bodyguard, he'll make her the most important girl in his world!",
            PublishYear = 2018,
            Categories = new List<Category>()
            {
                comedy,
                romance,
                schoolLife,
                shoujo,
            },
            Authors = new List<Author>()
            {
                fuyuAmakura
            },
            Chapters = new List<Chapter>()
            {
                new Chapter { Name = "Chapter 1: Encounter With A High School Student" },
                new Chapter { Name = "Chapter 2: A great success!!" },
                new Chapter { Name = "Chapter 3: I became his bodyguard" }
            }
        };

        Manga tsuiNoTaimashi = new ()
        {
            OriginalTitle = "Tsui no Taimashi -Endaa Gaisutaa- (終の退魔師―エンダーガイスター)",
            OriginalLanguage = Language.Japanese,
            AlternativeTitles = "The Last Exorcist -Ender Geister-",
            Description = "This protagonist is the most demonically strong exorcist. Akira, the S-class exorcist, sent from Germany to investigate a paranormal event in a certain city in Japan. He shoots creatures from other worlds with his beautiful exorcist partner Chikage.",
            PublishYear = 2019,
            Categories = new List<Category>()
            {
                action,
                drama,
                mature,
                mystery,
                shounen,
                supernatural,
            },
            Authors = new List<Author>()
            {
                yomoyamaTakashi
            },
            Chapters = new List<Chapter>()
            {
                new Chapter { Name = "Chapter 1: August eight" },
                new Chapter { Name = "Chapter 2: Con air" },
                new Chapter { Name = "Chapter 3: Eastern Promises" }
            }
        };

        Manga detectiveConan = new ()
        {
            OriginalTitle = "Meitantei Conan (名探偵コナン)",
            OriginalLanguage = Language.Japanese,
            AlternativeTitles = "Case Closed; 명탐정 코난; 名侦探柯南; Thám Tử Lừng Danh Conan",
            Description = "Shinichi Kudo is a high school detective who sometimes works with the police to solve cases. During an investigation, he is attacked by members of a crime syndicate known as the Black Organization. They force him to ingest an experimental poison, but instead of killing him, the poison transforms him into a child. Adopting the pseudonym Conan Edogawa and keeping his true identity a secret, Kudo lives with his childhood friend Ran and her father Kogoro, who is a private detective.",
            PublishYear = 1994,
            Categories = new List<Category>()
            {
                action,
                adventure,
                comedy,
                detective,
                drama,
                mystery,
                shounen,
            },
            Authors = new List<Author>()
            {
                goshoAoyama
            },
            Chapters = new List<Chapter>()
            {
                new Chapter { Name = "Chapter 1: The Heisei Holmes" },
                new Chapter { Name = "Chapter 2: The shrunken detective" },
                new Chapter { Name = "Chapter 3: The left out detective" }
            }
        };

        Manga yaiteruFutari = new()
        {
            OriginalTitle = "Yaiteru Futari (焼いてるふたり)",
            OriginalLanguage = Language.Japanese,
            AlternativeTitles = "A Rare Marriage: How to Grill Our Love; 굽고 있는 두 사람; 爱情是烤肉的滋味!",
            Description = "After their fateful encounter through a dating app, Kenta and Chihiro decided to get married with zero days of dating in light of Kenta's transfer. As a couple who got married without knowing much about each other, they deepened their relationship through their regular weekend BBQs.",
            PublishYear = 2020,
            Categories = new List<Category>()
            {
                cooking,
                romance,
                sliceOfLife,
            },
            Authors = new List<Author>()
            {
                hanatsukaShiori 
            },
            Chapters = new List<Chapter>()
            {
                new Chapter { Name = "Chapter 1: You apply just the right amount of heat" },
                new Chapter { Name = "Chapter 2: First breakfast together" },
                new Chapter { Name = "Chapter 3: A Toast with Beer and lamp chops" }
            }
        };

        _context.AddRange(doraemon, naruto, Komi_san_wa_Komyushou_Desu, The_Legendary_Moonlight_Sculptor, itouJunjiCollection, heroineHajimemashita, tsuiNoTaimashi, detectiveConan, yaiteruFutari);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
