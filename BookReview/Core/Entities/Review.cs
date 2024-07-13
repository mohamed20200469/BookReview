namespace BookReview.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
