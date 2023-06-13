using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public partial class SeedData
{
    private Dictionary<string, Category> SeedCategories()
    {
        Dictionary<string, Category> categories = new()
        {
            ["Action"] = new()
            {
                Name = "Action",
                Description = "The action genre is a class of creative works characterized by more emphasis on exciting action sequences than on character development or story-telling. It is usually possible to tell from the creative style of an action sequence, the genre of the entire creative work. For example, the style of a combat sequence will indicate whether the entire work is an action-adventure or martial art."
            },
            ["Adventure"] = new()
            {
                Name = "Adventure",
                Description = "The focus is on the hero's literal journey towards some goal and all the wacky stuff that happens along the way. These animated works often feature fantastical, imaginative worlds and intricate storylines that explore themes such as love, adventure, friendship, and personal growth."
            },
            ["Comedy"] = new()
            {
                Name = "Comedy",
                Description = "A genre that focuses on humor and laughter."
            },
            ["Cooking"] = new()
            {
                Name = "Cooking",
                Description = "A genre of anime that focuses on food and cooking, showcasing the daily lives of those who work in the food industry and highlighting various aspects of food appreciation and presentation. These anime shows feature characters who are passionate about cooking and baking, and they explore different cooking techniques, recipes, and culinary cultures from around the world."
            },
            ["Detective"] = new()
            {
                Name = "Detective",
                Description = "A genre that revolves around investigations, crime-solving, and detectives. This genre typically features a detective or a group of detectives who solve mysteries and crimes through their intelligence and skills."
            },
            ["Drama"] = new()
            {
                Name = "Drama",
                Description = "Focuses on serious and emotional themes, often portraying characters dealing with difficult situations or overcoming personal struggles."
            },
            ["Fantasy"] = new()
            {
                Name = "Fantasy",
                Description = "A genre that typically involves magical elements and is often set in a fictional universe. This also revolves around magic and supernatural events, featuring magical creatures and settings that may not be possible in reality."
            },
            ["Game"] = new()
            {
                Name = "Game",
                Description = "There are various game categories in anime, including anime-based games and game anime. Anime-based games are computer and video games that are based on manga or anime properties, while game anime showcases people's enthusiasm and love for gaming, regardless of the type of game. Game anime often shows characters growing and developing deeper strategies, building their confidence as players, and mastering moves that once seemed impossible."
            },
            ["Historical"] = new()
            {
                Name = "Historical",
                Description = "A genre that is set in a past time period and often features historical events, people, or places. These anime can be either serious or comedic, and they can take place in different parts of the world."
            },
            ["Horror"] = new()
            {
                Name = "Horror",
                Description = "A genre that features elements of fear, terror, and the macabre. Horror can include supernatural occurrences, monsters, ghosts, and serial killers."
            },
            ["Isekai"] = new()
            {
                Name = "Isekai",
                Description = "A genre that features displaced protagonists who are transported to and need to survive in another world, such as a fantasy world, virtual world, or parallel universe. This genre can be divided into different types based on the plot and storylines."
            },
            ["Josei"] = new()
            {
                Name = "Josei",
                Description = "A genre of anime and manga that is targeted toward adult women. It deals with realistic and complex themes such as work-life balance, relationships, and social issues in a mature and nuanced way."
            },
            ["Martial Arts"] = new()
            {
                Name = "Martial Arts",
                Description = "Martial arts series often showcase the unique fighting styles of different characters and blend real-world techniques with supernatural abilities and excellent choreography. Some series use martial arts in unique ways to create character-driven stories, while others immerse the viewer in a world of fighters and warriors."
            },
            ["Mature"] = new()
            {
                Name = "Mature",
                Description = "A genre that contains content that is not suitable for younger audiences. This includes mature themes such as violence, sexual content, and profanity. Mature series often explore complex and thought-provoking topics, such as politics, psychology, and philosophy. These mangas are typically targeted towards an older demographic and feature more realistic and nuanced portrayals of characters and situations."
            },
            ["Mecha"] = new()
            {
                Name = "Mecha",
                Description = "Focuses on giant robots or mechs, typically piloted by humans, engaged in battles or other types of action. Mecha stories often incorporate elements of science fiction, action, adventure, and drama, and can also feature political or philosophical themes."
            },
            ["Military"] = new()
            {
                Name = "Military",
                Description = "Stories that revolve around military organizations, personnel, and warfare. These stories often feature historical or futuristic settings and explore themes of patriotism, honor, sacrifice, and the human cost of war."
            },
            ["Mystery"] = new()
            {
                Name = "Mystery",
                Description = "Stories that involve a puzzle or a problem that needs to be solved. The plot typically centers around a crime or a series of crimes that must be deciphered by the protagonist or a group of characters. Mysteries can range from simple whodunits to complex stories that require the reader to solve the puzzle along with the characters. The genre often features twists and turns, red herrings, and unexpected revelations that keep the reader guessing until the very end."
            },
            ["Oneshot"] = new()
            {
                Name = "Oneshot",
                Description = "A standalone story that is published in a single chapter or issue. These stories are self-contained, with a complete plot, characters, and setting, and are not part of a larger series or ongoing story. Oneshots can be of any genre and can vary in length, from a few pages to several chapters. They are often used by manga creators as a way to experiment with new ideas, test the waters with readers, or showcase their skills."
            },
            ["Psychological"] = new()
            {
                Name = "Psychological",
                Description = "Focus on the internal struggles and mental states of the characters. These stories often explore complex themes such as trauma, mental illness, and the human psyche."
            },
            ["Romance"] = new()
            {
                Name = "Romance",
                Description = "Focus on characters who are navigating their feelings and emotions, as well as the feelings of those around them. This makes for a relatable story that is both entertaining and thought-provoking. In addition to exploring love and relationships, Romance manga also explores themes such as family, friendship, self-discovery, and growth. These stories can be very inspiring to readers of all ages, as they demonstrate how characters can overcome obstacles to find happiness."
            },
            ["School Life"] = new()
            {
                Name = "School Life",
                Description = "Focus on the experiences of characters in a high school setting. This genre typically explores themes such as self-discovery, personal image, friendships, and romance. The manga in this category showcase events that occur on a daily basis in a school, whether from the perspective of a student or of a teacher."
            },
            ["Sci-Fi"] = new()
            {
                Name = "Sci-Fi",
                Description = "A category that explores new worlds, either in the future or past, on Earth or other planets. It features humanity's exploration of new technologies, societies, or frontiers in outer space. This category is based on imagination and creativity and often involves imagined technological advancements or natural settings."
            },
            ["Seinen"] = new()
            {
                Name = "Seinen",
                Description = "A category that is marketed towards adult men between the ages of 18 and 40. It covers a wide range of genres such as action, politics, science fiction, fantasy, relationships, sports, or comedy. It is often more sophisticated, psychological, satirical, violent, sexual, and for a more mature audience."
            },
            ["Shoujo"] = new()
            {
                Name = "Shoujo",
                Description = "A genre targeted towards young girls and women. It typically features romantic relationships, emotional drama, and character development. The stories often center around a female protagonist and explore themes such as love, friendship, and self-discovery. Shoujo mangas are known for their unique art style, which often includes large, expressive eyes and intricate detail work."
            },
            ["Shounen"] = new()
            {
                Name = "Shounen",
                Description = "A category of manga marketed towards adolescent boys, characterized by high-action plots featuring male protagonists and emphasizing camaraderie among male characters. Shonen works often have more than a fair share of fanservice as well."
            },
            ["Slice of Life"] = new()
            {
                Name = "Slice of Life",
                Description = "A genre that portrays everyday life, often focusing on the mundane and ordinary experiences of the characters. These stories can be humorous, heartwarming, or serious and often explore the relationships between characters as they navigate their daily lives. Slice of Life manga typically does not have a central plot or conflict and instead focuses on the characters' experiences and emotions."
            },
            ["Sports"] = new()
            {
                Name = "Sports",
                Description = "A category in manga that features stories about athletes and their struggles to win in various sports competitions. This genre often focuses on themes of perseverance, teamwork, and overcoming obstacles."
            },
            ["Supernatural"] = new()
            {
                Name = "Supernatural",
                Description = "Focuses on supernatural elements such as ghosts, vampires, demons, and other supernatural beings. The stories in this category often involve battles between good and evil and feature characters with supernatural abilities or powers."
            },
            ["Thriller"] = new()
            {
                Name = "Thriller",
                Description = "A genre that focuses on suspense, excitement, and anticipation. The stories usually revolve around a central mystery or problem that the protagonist must solve. The genre is known for its twists and turns, unexpected plot developments, and fast-paced action."
            },
            ["Tragedy"] = new()
            {
                Name = "Tragedy",
                Description = "A genre that deals with serious and often depressing subject matter. It typically involves the downfall or suffering of the protagonist or other characters and explores themes of loss, despair, and the fragility of human existence."
            }
        };

        _context.Categories.AddRange(categories.Values);
        return categories;
    }
}
