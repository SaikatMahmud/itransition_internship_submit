using Microsoft.AspNetCore.SignalR;
using System.Xml.Linq;

namespace DrawingBoard.Hubs
{
    public class BoardHub : Hub
    {
        public async Task Draw(string boardId, string drawData)
        {
           await Clients.OthersInGroup(boardId).SendAsync("UpdateDrawing", drawData);
        }

        public async Task<string> JoinBoard(string boardId, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);
            await Clients.OthersInGroup(boardId).SendAsync("ReceiveUserJoinInfo", username);
            return $"Added to group with connection id {Context.ConnectionId} in group {boardId}";
        }
    }
}
