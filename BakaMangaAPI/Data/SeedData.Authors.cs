﻿using BakaMangaAPI.Models;

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
                Biography = "Fujiko Fujio was a pen name of a manga writing duo formed by two Japanese manga artists. Their real names are Hiroshi Fujimoto (1933–1996) and Motoo Abiko (1934-2022). They formed their partnership in 1951, and used the Fujiko Fujio name from 1954 until dissolution of the partnership in 1987."
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
                Biography = "Junji Ito (born July 31, 1963) is a Japanese horror manga artist. Some of his most notable works include Tomie, a series chronicling an immortal girl who drives her stricken admirers to madness; Spiral Into Horror Uzumaki, a three-volume series about a town obsessed with spirals; and Gyo, a two-volume story where fish are controlled by a strain of sentient bacteria called \"the death stench.\" "
            },
            ["Gosho Aoyama"] = new()
            {
                Name = "Gosho Aoyama",
                Biography = "Gosho Aoyama (born June 21, 1963) is a Japanese manga artist best known for his manga series Detective Conan (1994–present), known as Case Closed in some English-speaking countries. As of 2017, his various manga series had a combined 200 million copies in print worldwide. Aoyama made his debut as a manga artist with Chotto Mattete, which was published in the magazine Weekly Shōnen Sunday in winter of 1987. "
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
