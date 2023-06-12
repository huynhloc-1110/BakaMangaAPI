using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public partial class SeedData
{
    private Dictionary<string, Author> SeedAuthors()
    {
        Dictionary<string, Author> authors = new()
        {
            ["Masashi Kishimoto"] = new()
            {
                Name = "Masashi Kishimoto",
                Biography = "Masashi Kishimoto, born November 8, 1974, is a Japanese manga artist. His best-known work, Naruto, was in serialization from 1999 to 2014 and has sold over 250 million copies worldwide in 46 countries as of May 2019."
            },
            ["Fujiko F. Fujio"] = new()
            {
                Name = "Fujiko F. Fujio",
                Biography = "Fujiko Fujio was a pen name of a manga writing duo formed by two Japanese manga artists. Their real names are Hiroshi Fujimoto (1933-1996) and Motoo Abiko (1934-2022). They formed their partnership in 1951 and used the Fujiko Fujio name from 1954 until the dissolution of the partnership in 1987. From the outset, they adopted a collaborative style where both worked simultaneously on the story and artwork, but as they diverged creatively they started releasing individual works under different names, Abiko as Fujiko Fujio, and Fujimoto as Fujiko F. Fujio. Throughout their career, they won many individual and collaborative awards, and are best known for creating the popular and long-running series Doraemon, the main character of which is officially recognized as a cultural icon of modern Japan. Some influences of most of their projects are the works of acclaimed manga artist Osamu Tezuka and many US cartoons and comic books—including the works of Hanna-Barbera."
            },
            ["Oda Tomohito"] = new()
            {
                Name = "Oda Tomohito",
                Biography = "Oda Tomohito won the grand prize for World Worst One in the 70th Shogakukan New Comic Artist Awards in 2012. Oda's series Digicon, about a tough high school who finds herself in control of an alien with plans for world domination, ran from 2014 to 2015. In 2015, Komi-San Wa Komyushou Desu debuted as a one-shot in Weekly Shounen Sunday and was picked up as a full series by the same magazine in 2016."
            },
            ["Heesung NAM"] = new()
            {
                Name = "Heesung NAM",
                Biography = "Heesung NAM (October 5, 1980) is famously known as the author of The Legendary Moonlight Sculptor, a Korean game fantasy novel that has spanned 58 volumes since 2007."
            },
            ["Itou Junji"] = new()
            {
                Name = "Itou Junji",
                Biography = "Junji Ito (born July 31, 1963) is a Japanese horror manga artist. Some of his most notable works include Tomie, a series chronicling an immortal girl who drives her stricken admirers to madness; Spiral Into Horror Uzumaki, a three-volume series about a town obsessed with spirals; and Gyo, a two-volume story where fish are controlled by a strain of sentient bacteria called 'the death stench.' His other works include Itou Junji Kyoufu Manga Collection, a collection of different short stories including a series of stories named Souichi's Journal of Delights, and Junji Ito's Cat Diary: Yon & Mu, a self-parody about him and his wife living in a house with two cats. Ito's work has developed a substantial cult following, with some deeming him a significant figure in recent horror iconography."
            },
            ["Gosho Aoyama"] = new()
            {
                Name = "Gosho Aoyama",
                Biography = "Gosho Aoyama (born June 21, 1963) is a Japanese manga artist best known for his manga series Detective Conan (1994–present), known as Case Closed in some English-speaking countries. As of 2017, his various manga series had a combined 200 million copies in print worldwide. Aoyama made his debut as a manga artist with Chotto Mattete, which was published in the magazine Weekly Shōnen Sunday in winter of 1987. [Shortly after that, he began Magic Kaito in the same magazine. Magic Kaito protagonist Kaito Kuroba later appeared in Case Closed. Between 1988 and 1993, Aoyama created the series Yaiba, which ran for 24 volumes. Later, he would release other manga series in single volumes, such as Third Baseman No.4 and Gosho Aoyama's Collection of Short Stories. Aoyama began serializing Detective Conan in Weekly Shōnen Sunday on January 19, 1994. When the series was first released in English, it was given the title Case Closed."
            },
            ["Fuyu Amakura"] = new()
            {
                Name = "Fuyu Amakura",
                Biography = "You can find more about the author on their Twitter account: [Fuyu Amakura on Twitter](https://twitter.com/amakura_fuyu)"
            },
            ["YOMOYAMA Takashi"] = new()
            {
                Name = "YOMOYAMA Takashi",
                Biography = "YOMOYAMA Takashi's mother is from Germany. He attended a Japanese school in Germany up until Junior School before moving to Japan and continuing High School and University Education."
            },
            ["Hanatsuka Shiori"] = new()
            {
                Name = "Hanatsuka Shiori",
                Biography = "You can find more about the author on their Twitter account: [Hanatsuka Shiori on Twitter](https://twitter.com/hntkchan)"
            }
        };

        _context.Authors.AddRange(authors.Values);
        return authors;
    }
}
