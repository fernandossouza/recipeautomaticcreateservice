namespace recipeautomaticcreateservice.Model
{
    public class Recipe
    {
        public int recipeId { get; set; }
        public string recipeName { get; set; }
        public string recipeDescription { get; set; }
        public string recipeCode { get; set; }
        public int[] phasesId { get; set; }
    }
}