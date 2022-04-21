using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeUrDJ_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using BeUrDJ_MVC.Service;
using Microsoft.AspNetCore.Http;
using BeUrDJ_MVC.Models.SQL;
using BeUrDJ_MVC.Models.ViewModels;
using BeUrDJ_MVC.Controllers;

namespace BeUrDJ_MVC.Hubs
{

    public class QueueHub : Hub
    {
        private SongService _songService = new SongService();
        private AccountService _accountService = new AccountService();

        public async Task UpdateQueue(int sessionId)
        {
            //refactor the model to a viewmodel to include the session - session can then be used anywhere on the page. 
            await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQueue");
        }
        public async Task UpdateLikes(int songId, int likes, int sessionId, int userId)
        {
            _songService.UpdateLikes(songId, likes, sessionId, userId);
            //refactor the model to a viewmodel to include the session - session can then be used anywhere on the page. 
            await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQueue");
        }
        public async Task UpdateListeners(int sessionId, int userId)
        {
            Clients.Group(sessionId.ToString());
            //refactor the model to a viewmodel to include the session - session can then be used anywhere on the page. 
            await Clients.All.SendAsync("ReceiveQueue");
        }
        public async Task JoinQueue(int sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());
        }
        public async Task ExitQueue(int sessionId, int userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId.ToString());
        }
        public async Task CreateQueue(int sessionId, int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());
        }
    }
}
