using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TicTacToeMVC.Hubs
{
    public class TicTacToeHub : Hub
    {
        public async Task MakeMove(string player, int row, int col)
        {
            await Clients.Others.SendAsync("ReceiveMove", player, row, col);
        }

        public async Task ResetGame()
        {
            await Clients.All.SendAsync("GameReset");
        }

        public async Task UpdateScoreboard(int xWins, int oWins, int draws)
        {
            await Clients.All.SendAsync("UpdateScoreboard", xWins, oWins, draws);
        }
    }
}
