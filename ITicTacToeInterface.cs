public interface ITicTacToeAI
{
    void InitializeAI(int size, char aiChar, char humanChar, int depth = 4);
    (int, int) GetBestMove(char[,] board);
}