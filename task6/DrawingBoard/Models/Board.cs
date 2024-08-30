using System.ComponentModel.DataAnnotations;

namespace DrawingBoard.Models
{
    public class Board
    {
        [Key]
        public int BoardId { get; set; }
        public byte[]? SvgData { get; set; }
    }
}
