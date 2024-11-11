using ArbitraryCollectionMgmt.BLL.DTOs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Hubs
{
    public class CommentHub : Hub<ICommentHub>
    {
        public async Task<string> JoinGroup(string itemId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, itemId);
            return $"Added to group with connection id {Context.ConnectionId} in group {itemId}" ;
        }

        public async Task SendNewComment(CommentDTO comment)
        {
            await Clients.Group(comment.ItemId.ToString()).ReceiveComment(comment);
        }

    }
}
