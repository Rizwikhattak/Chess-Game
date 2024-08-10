using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using PawnUpdation;
namespace ChessGame
{
    public partial class Form1 : Form
    {


        /*string imagePath = @"C:\Users\Muhammad Rizwan\Desktop\Chess\Chess\ChessUI\Assets\BishopB.png";*/

        TcpClient client;
        BinaryFormatter formatter;
        Stream stream;

         Button[,] AllButtons;
        
        Dictionary<string, List<List<int>>> calcMoves = new Dictionary<string, List<List<int>>>();
        int checkInvoke = 0;
        bool isBlackTurn = false; // True if it's white's turn, false if it's black's turn
        int updatePawn = 0;
        // Dictionary to store piece images
        Dictionary<string, Image> pieceImages = new Dictionary<string, Image>();

        public Form1()
        {
            InitializeComponent();
            this.Text = "Player 2(Client)";
            AllButtons = new Button[8, 8];
            LoadPieceImages(); // Load images
            IPEndPoint clientEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"),6001);
            client = new TcpClient(clientEp);

            IPEndPoint serverEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"),6000);
            client.Connect(serverEp);
            Console.WriteLine("Connected to the server");
            stream = client.GetStream();
            formatter = new BinaryFormatter();
            RecieveServersData();



        }

        public void UpdatePawn(int endRow, int endCol)
        {
            pawnUpdation pawn = new pawnUpdation();
            pawn.ShowDialog();
            AllButtons[endRow, endCol].Image = pawn.img;
            AllButtons[endRow, endCol].Name = pawn.pieceType;
            if (pawn.pieceType == "QUEEN_BLACK")
                updatePawn = 1;
            else if (pawn.pieceType == "BISHOP_BLACK")
                updatePawn = 2;
            else if (pawn.pieceType == "KNIGHT_BLACK")
                updatePawn = 3;
            else if (pawn.pieceType == "ROOK_BLACK")
                updatePawn = 4;

        }

        public void RecieveServersData()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        List<int> RecvieveServerData = formatter.Deserialize(stream) as List<int>;
                        UpdateBoard(RecvieveServerData);
                        isBlackTurn = !isBlackTurn;
                    }catch(Exception e)
                    {
                        
                    }
                }
            });


        }

        public void UpdateBoard(List<int> RecvieveServerData)
        {
            int startRow = RecvieveServerData[0];
            int startCol = RecvieveServerData[1];

            int endRow = RecvieveServerData[2];
            int endCol = RecvieveServerData[3];

            AllButtons[endRow, endCol].Image = AllButtons[startRow, startCol].Image;
            AllButtons[endRow, endCol].Name = AllButtons[startRow, startCol].Name;

            AllButtons[startRow, startCol].Image = null;
            AllButtons[startRow, startCol].Name = "";

            if (AllButtons[endRow, endCol].Name.Contains("PAWN_WHITE") && endRow == 7)
            {
                Console.WriteLine("IN PAWN WHITEEEEEEEEEEEEEEEEE");
                if (RecvieveServerData[4] == 1)
                {
                    SetPiece(AllButtons[endRow, endCol], "QUEEN_WHITE");
                }
                else if (RecvieveServerData[4] == 2)
                {
                    SetPiece(AllButtons[endRow, endCol], "BISHOP_WHITE");
                }
                else if (RecvieveServerData[4] == 3)
                {
                    SetPiece(AllButtons[endRow, endCol], "KNIGHT_WHITE");
                }
                else if (RecvieveServerData[4] == 4)
                {
                    SetPiece(AllButtons[endRow, endCol], "ROOK_WHITE");
                }
                else
                {
                    Console.WriteLine("ZEROOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                }
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeButtonsList();
        }



        private void LoadPieceImages()
        {
            pieceImages["PAWN_WHITE"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Pawn_White.png");
            pieceImages["ROOK_WHITE"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Rook_White.png");
            pieceImages["KNIGHT_WHITE"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Knight_White.png");
            pieceImages["BISHOP_WHITE"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Bishop_White.png");
            pieceImages["QUEEN_WHITE"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Queen_White.png");
            pieceImages["KING_WHITE"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\King_White.png");

            pieceImages["PAWN_BLACK"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Pawn_Black.png");
            pieceImages["ROOK_BLACK"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Rook_Black.png");
            pieceImages["KNIGHT_BLACK"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Knight_Black.png");
            pieceImages["BISHOP_BLACK"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Bishop_Black.png");
            pieceImages["QUEEN_BLACK"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Queen_Black.png");
            pieceImages["KING_BLACK"] = Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\King_Black.png");
        }

        private void InitializeButtonsList()
        {
            var buttons = this.Controls.OfType<Button>().Reverse().ToArray();
            int row = 0, col;

            for (int i = 0; i < buttons.Count(); i++)
            {
                col = i % AllButtons.GetLength(0);

                buttons[i].Font = new Font("Arial", 12, FontStyle.Bold);
                buttons[i].Text = "";
                buttons[i].TextAlign = ContentAlignment.MiddleCenter;
                buttons[i].Click += button_Click;

                AllButtons[row, col] = buttons[i];

                if (col == 7)
                    row++;
            }

            for (int i = 0; i < AllButtons.GetLength(0); i++)
            {
                for (int j = 0; j < AllButtons.GetLength(1); j++)
                {
                    AllButtons[i, j].Tag = new List<int> { i, j };

                    if (i % 2 != 0)
                    {
                        if (j % 2 != 0)
                        {
                            AllButtons[i, j].BackColor = Color.Black;
                            AllButtons[i, j].ForeColor = Color.White;
                        }
                            
                        
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            AllButtons[i, j].BackColor = Color.Black;
                            AllButtons[i, j].ForeColor = Color.White;
                        }
                    }
                }
            }

            /*for (int i = 0; i < AllButtons.GetLength(0); i++)
            {
                for (int j = 0; j < AllButtons.GetLength(1); j++)
                {
                    if (i % 2 != 0)
                        if (j % 2 != 0)
                            AllButtons[i, j].ForeColor = Color.White;
                }
            }*/

            string[] pieces = new string[] { "ROOK", "KNIGHT", "BISHOP", "QUEEN", "KING", "BISHOP", "KNIGHT", "ROOK", "PAWN" };
            for (int i = 0; i < 8; i++)
            {
                SetPiece(AllButtons[0, i], $"{pieces[i]}_WHITE");
                SetPiece(AllButtons[1, i], "PAWN_WHITE");

                SetPiece(AllButtons[7, i], $"{pieces[i]}_BLACK");
                SetPiece(AllButtons[6, i], "PAWN_BLACK");
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }


        private void SetPiece(Button button, string pieceType)
        {
            int imageSize = 40; // Adjust this size to fit your button
            Image resizedImage = ResizeImage(pieceImages[pieceType], imageSize, imageSize);
            button.Image = resizedImage;
            button.ImageAlign = ContentAlignment.MiddleCenter; // Center the image
            button.Name = pieceType;
        }



        public void movePiece(object sender, EventArgs e)
        {
            if (checkInvoke == 0)
            {
                Console.WriteLine("In Move Piece");
                string pieceType = calcMoves.Keys.FirstOrDefault();
                int startRow = calcMoves[pieceType][0][0];
                int startCol = calcMoves[pieceType][0][1];

                Button clickedButton = (Button)sender;
                List<int> indexes = clickedButton.Tag as List<int>;
                int endRow = indexes[0];
                int endCol = indexes[1];



                List<int> serverData = new List<int>() { startRow, startCol, endRow, endCol };
                UpdateBoard(serverData);

                if (pieceType.Contains("PAWN_BLACK") && endRow == 0)
                {
                    UpdatePawn(endRow, endCol);
                }
                serverData.Add(updatePawn);
                formatter.Serialize(stream, serverData);


                RemoveMovePieceFunctionality();
                RemoveGreenColorFunctionality(pieceType);
                calcMoves.Clear();
                checkInvoke++;

                // Toggle the turn flag
                isBlackTurn = !isBlackTurn;
            }

        }

        private void ClearEventHandlers()
        {
            string piece = calcMoves.Keys.FirstOrDefault();



            if (piece != null)
            {
                Console.WriteLine("Trying to remove click");
                for (int i = 1; i < calcMoves[piece].Count; i++)
                {

                    int endRow = calcMoves[piece][i][0];
                    int endCol = calcMoves[piece][i][1];

                    AllButtons[endRow, endCol].Click -= movePiece;

                }
                RemoveGreenColorFunctionality(piece);

                /*ClearEventHandlers(AllButtons[endRow,endCol], "Click");*/
            }
            calcMoves.Clear();
            checkInvoke = 0;
        }


        private void button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check whose turn it is
            string pieceType = clickedButton.Name.ToString();
            bool isBlackPiece = pieceType.Contains("BLACK");
            if (!isBlackTurn && isBlackPiece)
            {
                // It's not the current player's turn
                MessageBox.Show("It's not your turn!");
                return;
            }

            List<int> indexes = clickedButton.Tag as List<int>;
            int row = indexes[0];
            int col = indexes[1];

            ClearEventHandlers();

            HandlePiece(pieceType, row, col);
        }

        public void DisplayGreenOnPossibleMoves(List<List<int>> calcPossibleIndexes)
        {
            calcPossibleIndexes.RemoveAt(0);
            foreach (var loc in calcPossibleIndexes)
            {
                int row = loc[0];
                int col = loc[1];
                AllButtons[row, col].BackColor = Color.Green;
            }
        }
        public void RemoveGreenColorFunctionality(string pieceType)
        {

            foreach (var loc in calcMoves[pieceType])
            {
                int row = loc[0];
                int col = loc[1];
                if (row % 2 != 0)
                {
                    if (col % 2 != 0)
                    {
                        AllButtons[row, col].BackColor = Color.Black;
                        AllButtons[row, col].ForeColor = Color.White;
                    }


                    else
                    {
                        Console.WriteLine("IN FIIIIIRST ELSEEE");
                        AllButtons[row, col].BackColor = Color.White;
                        AllButtons[row, col].ForeColor = Color.Black;
                    }

                }
                else
                {
                    if (col % 2 == 0)
                    {
                        AllButtons[row, col].BackColor = Color.Black;
                        AllButtons[row, col].ForeColor = Color.White;
                    }


                    else
                    {
                        Console.WriteLine("IN Seconnnddddd ELSEEE");
                        AllButtons[row, col].BackColor = Color.White;
                        AllButtons[row, col].ForeColor = Color.Black;
                    }

                }

            }
        }

        public void HandlePiece(string pieceType,int row,int col)
        {
            
            switch(pieceType)
            {
                case "PAWN_BLACK":
                    HandlePawn(pieceType,row, col);                   
                    break;
                case "ROOK_BLACK":
                    HandleRook(pieceType, row, col);
                    break;
                case "BISHOP_BLACK":
                    HandleBishop(pieceType, row, col);
                    break;
                case "KNIGHT_BLACK":
                    HandleKnight(pieceType, row, col);
                    break;
                case "QUEEN_BLACK":
                    HandleQueen(pieceType, row, col);
                    break;
                case "KING_BLACK":
                    HandleKing(pieceType,row,col);
                    break;
                
                default:
                    Console.WriteLine("Invalid Piece");
                    
                    break;

            }
            

        }

        private bool IsValidMove(string pieceType, int startRow, int startCol, int endRow, int endCol)
        {
            // Check if the end position is within the board limits
            if (endRow < 0 || endRow >= 8 || endCol < 0 || endCol >= 8)
            {
                return false;
            }

            // Check if there is an image (piece) at the end position
            if (AllButtons[endRow, endCol].Image != null)
            {
                // Check if it's an opponent's piece
                string endPieceType = AllButtons[endRow, endCol].Name;
                if (!endPieceType.Contains(pieceType.Split('_')[1]))
                {
                    return true; // Valid move, capture opponent's piece
                }
                return false; // Invalid move, own piece
            }

            return true; // Valid move, empty square
        }
        



        public void HandleKing(string pieceType,int row,int col)
        {
            CalculateKingMoves(pieceType, row, col);
        }
        public void CalculateKingMoves(string pieceType, int row, int col)
        {
            List<List<int>> calcPossibleIndexes = new List<List<int>>()
            {
                new List<int> { row, col },
            };

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("King Moves");

            // All possible moves a king can make (one square in any direction)
            int[][] moves = new int[][]
            {
                new int[] { row + 1, col },     // Move forward
                new int[] { row - 1, col },     // Move backward
                new int[] { row, col + 1 },     // Move right
                new int[] { row, col - 1 },     // Move left
                new int[] { row + 1, col + 1 }, // Move up-right
                new int[] { row + 1, col - 1 }, // Move up-left
                new int[] { row - 1, col + 1 }, // Move down-right
                new int[] { row - 1, col - 1 }  // Move down-left
            };

            foreach (var move in moves)
            {
                int newRow = move[0];
                int newCol = move[1];

                // Check if the move is within the board limits
                if (IsValidMove(pieceType, row, col, newRow, newCol))
                {
                    string enemyName = AllButtons[newRow, newCol].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        continue;
                    Console.WriteLine($"{newRow} {newCol}");
                    calcPossibleIndexes.Add(new List<int>() { newRow, newCol });

                    AllButtons[newRow, newCol].Click += movePiece;
                    AllButtons[newRow, newCol].Click -= button_Click;
                }
            }

            calcMoves.Add(pieceType, calcPossibleIndexes);
            DisplayGreenOnPossibleMoves(new List<List<int>>(calcPossibleIndexes));
            checkInvoke = 0;
            Console.WriteLine();
        }


        public void HandleQueen(string pieceType,int row,int col)
        {
            CalculateQueenMoves(pieceType, row, col);
        }

        public void CalculateQueenMoves(string pieceType, int row, int col)
        {
            List<List<int>> calcPossibleIndexes = new List<List<int>>()
            {
                new List<int> { row, col },
            };

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Queen Moves");

            // Combining Rook and Bishop moves

            // Vertical and Horizontal Moves (Rook part)
            Console.WriteLine("Vertical and Horizontal Moves");

            // Forward Moves
            for (int i = row + 1; i < 8; i++)
            {
                if (IsValidMove(pieceType, row, col, i, col))
                {
                    string enemyName = AllButtons[i, col].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, col });
                    AllButtons[i, col].Click += movePiece;
                    AllButtons[i, col].Click -= button_Click;
                    if (AllButtons[i, col].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Backward Moves
            for (int i = row - 1; i >= 0; i--)
            {
                if (IsValidMove(pieceType, row, col, i, col))
                {
                    string enemyName = AllButtons[i, col].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, col });
                    AllButtons[i, col].Click += movePiece;
                    AllButtons[i, col].Click -= button_Click;
                    if (AllButtons[i, col].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Rightward Moves
            for (int i = col + 1; i < 8; i++)
            {
                if (IsValidMove(pieceType, row, col, row, i))
                {
                    string enemyName = AllButtons[row, i].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { row, i });
                    AllButtons[row, i].Click += movePiece;
                    AllButtons[row, i].Click -= button_Click;
                    if (AllButtons[row, i].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Leftward Moves
            for (int i = col - 1; i >= 0; i--)
            {
                if (IsValidMove(pieceType, row, col, row, i))
                {
                    string enemyName = AllButtons[row, i].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { row, i });
                    AllButtons[row, i].Click += movePiece;
                    AllButtons[row, i].Click -= button_Click;
                    if (AllButtons[row, i].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Diagonal Moves (Bishop part)
            Console.WriteLine("Diagonal Moves");

            // Up-Right Diagonal Moves
            for (int i = row + 1, j = col + 1; i < 8 && j < 8; i++, j++)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Up-Left Diagonal Moves
            for (int i = row + 1, j = col - 1; i < 8 && j >= 0; i++, j--)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Down-Right Diagonal Moves
            for (int i = row - 1, j = col + 1; i >= 0 && j < 8; i--, j++)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Down-Left Diagonal Moves
            for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;

                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Text != "")
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            calcMoves.Add(pieceType, calcPossibleIndexes);
            DisplayGreenOnPossibleMoves(new List<List<int>>(calcPossibleIndexes));
            checkInvoke = 0;
            Console.WriteLine();
        }



        public void HandleKnight(string pieceType, int row, int col)
        {
            CalculateKnightMoves(pieceType, row, col);
        }
        public void CalculateKnightMoves(string pieceType, int row, int col)
        {
            List<List<int>> calcPossibleIndexes = new List<List<int>>()
            {
                new List<int> { row, col },
            };

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Knight Moves");

            // All possible moves a knight can make
            int[][] moves = new int[][]
            {
                new int[] { row + 2, col + 1 },
                new int[] { row + 2, col - 1 },
                new int[] { row - 2, col + 1 },
                new int[] { row - 2, col - 1 },
                new int[] { row + 1, col + 2 },
                new int[] { row + 1, col - 2 },
                new int[] { row - 1, col + 2 },
                new int[] { row - 1, col - 2 }
            };

            foreach (var move in moves)
            {
                int newRow = move[0];
                int newCol = move[1];

                if (IsValidMove(pieceType, row, col, newRow, newCol))
                {
                    string enemyName = AllButtons[newRow,newCol].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        continue;
                    calcPossibleIndexes.Add(new List<int>() { newRow, newCol });

                    AllButtons[newRow, newCol].Click += movePiece;
                    AllButtons[newRow, newCol].Click -= button_Click;
                }
            }

            calcMoves.Add(pieceType, calcPossibleIndexes);
            DisplayGreenOnPossibleMoves(new List<List<int>>(calcPossibleIndexes));
            checkInvoke = 0;
            Console.WriteLine();
        }




        public void HandleBishop(string pieceType, int row, int col)
        {
            CalculateBishopMoves(pieceType, row, col);
        }
        public void CalculateBishopMoves(string pieceType, int row, int col)
        {
            List<List<int>> calcPossibleIndexes = new List<List<int>>()
            {
                new List<int> { row, col },
            };

            Console.WriteLine("Diagonal Moves");
            Console.WriteLine("-----------------------------------------");

            // Up-Right Diagonal Moves
            for (int i = row + 1, j = col + 1; i < 8 && j < 8; i++, j++)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Up-Left Diagonal Moves
            for (int i = row + 1, j = col - 1; i < 8 && j >= 0; i++, j--)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Down-Right Diagonal Moves
            for (int i = row - 1, j = col + 1; i >= 0 && j < 8; i--, j++)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            // Down-Left Diagonal Moves
            for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (IsValidMove(pieceType, row, col, i, j))
                {
                    string enemyName = AllButtons[i, j].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, j });
                    AllButtons[i, j].Click += movePiece;
                    AllButtons[i, j].Click -= button_Click;
                    if (AllButtons[i, j].Image != null )
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            calcMoves.Add(pieceType, calcPossibleIndexes);
            DisplayGreenOnPossibleMoves(new List<List<int>>(calcPossibleIndexes));
            checkInvoke = 0;
            Console.WriteLine();
        }


        public void HandleRook(string pieceType,int row,int col)
        {
            CalculateRookMoves(pieceType, row, col);
        }
        public void CalculateRookMoves(string pieceType, int row, int col)
        {
            List<List<int>> calcPossibleIndexes = new List<List<int>>()
            {
                new List<int> { row, col },
            };

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Forward Possibilities");

            // Forward Moves
            for (int i = row + 1; i < 8; i++)
            {
                if (IsValidMove(pieceType, row, col, i, col))
                {
                    string enemyName = AllButtons[i, col].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, col });
                    AllButtons[i, col].Click += movePiece;
                    AllButtons[i, col].Click -= button_Click;
                    if (AllButtons[i, col].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Backward Possibilities");

            // Backward Moves
            for (int i = row - 1; i >= 0; i--)
            {
                if (IsValidMove(pieceType, row, col, i, col))
                {
                    string enemyName = AllButtons[i, col].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { i, col });
                    AllButtons[i, col].Click += movePiece;
                    AllButtons[i, col].Click -= button_Click;
                    if (AllButtons[i, col].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Rightward Possibilities");

            // Right Moves
            for (int i = col + 1; i < 8; i++)
            {
                if (IsValidMove(pieceType, row, col, row, i))
                {
                    string enemyName = AllButtons[row, i].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { row, i });
                    AllButtons[row, i].Click += movePiece;
                    AllButtons[row, i].Click -= button_Click;
                    if (AllButtons[row, i].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Leftward Possibilities");

            // Left Moves
            for (int i = col - 1; i >= 0; i--)
            {
                if (IsValidMove(pieceType, row, col, row, i))
                {
                    string enemyName = AllButtons[row, i].Name;
                    string firstName = enemyName.Split('_')[0];
                    if (firstName.Contains("KING"))
                        break;
                    calcPossibleIndexes.Add(new List<int>() { row, i });
                    AllButtons[row, i].Click += movePiece;
                    AllButtons[row, i].Click -= button_Click;
                    if (AllButtons[row, i].Image != null)
                    {
                        break; // Stop if a piece is found (capture or block)
                    }
                }
                else
                {
                    break; // Stop if move is invalid
                }
            }

            calcMoves.Add(pieceType, calcPossibleIndexes);
            DisplayGreenOnPossibleMoves(new List<List<int>>(calcPossibleIndexes));
            checkInvoke = 0;
            Console.WriteLine();
        }


        public void HandlePawn(string pieceType,int row,int col)
        {
            Console.WriteLine("In Handle Pawn");
            CalculatePawnMoves( pieceType , row,  col);
        }

        public void CalculatePawnMoves(string pieceType, int row, int col)
        {
            List<List<int>> calcPossibleIndexes = new List<List<int>>()
    {
        new List<int> { row, col },
    };

            Console.WriteLine("Pawn Moves");
            Console.WriteLine("-----------------------------------------");

            // Determine the direction of movement based on the color of the pawn
            int direction = pieceType.Contains("WHITE") ? 1 : -1;

            // Single square forward move
            if (IsValidMove(pieceType, row, col, row + direction, col) && AllButtons[row + direction, col].Image == null)
            {
                calcPossibleIndexes.Add(new List<int>() { row + direction, col });
                AllButtons[row + direction, col].Click += movePiece;
                AllButtons[row + direction, col].Click -= button_Click;
            }

            // Double square forward move (only from initial position)
            if ((row == 1 && direction == 1) || (row == 6 && direction == -1))
            {
                if (IsValidMove(pieceType, row, col, row + 2 * direction, col) &&
                    AllButtons[row + direction, col].Image == null &&
                    AllButtons[row + 2 * direction, col].Image == null)
                {
                    calcPossibleIndexes.Add(new List<int>() { row + 2 * direction, col });
                    AllButtons[row + 2 * direction, col].Click += movePiece;
                    AllButtons[row + 2 * direction, col].Click -= button_Click;
                }
            }

            // Diagonal captures
            if (IsValidMove(pieceType, row, col, row + direction, col + 1) && AllButtons[row + direction, col + 1].Image != null && !AllButtons[row + direction, col + 1].Name.Contains(pieceType.Split('_')[1]))
            {
                string enemyName = AllButtons[row+direction, col+1].Name;
                string firstName = enemyName.Split('_')[0];
                if (!firstName.Contains("KING"))
                {
                    calcPossibleIndexes.Add(new List<int>() { row + direction, col + 1 });
                    AllButtons[row + direction, col + 1].Click += movePiece;
                    AllButtons[row + direction, col + 1].Click -= button_Click;
                }
                    

                
            }

            if (IsValidMove(pieceType, row, col, row + direction, col - 1) && AllButtons[row + direction, col - 1].Image != null && !AllButtons[row + direction, col - 1].Name.Contains(pieceType.Split('_')[1]))
            {
                string enemyName = AllButtons[row + direction, col-1].Name;
                string firstName = enemyName.Split('_')[0];
                if (!firstName.Contains("KING"))
                {
                    calcPossibleIndexes.Add(new List<int>() { row + direction, col - 1 });
                    AllButtons[row + direction, col - 1].Click += movePiece;
                    AllButtons[row + direction, col - 1].Click -= button_Click;
                }

                  
            }

            calcMoves.Add(pieceType, calcPossibleIndexes);
            DisplayGreenOnPossibleMoves(new List<List<int>>(calcPossibleIndexes));
            checkInvoke = 0;
            Console.WriteLine();
        }




       

        public void RemoveMovePieceFunctionality()
        {
            string piece = calcMoves.Keys.FirstOrDefault();



            if (piece != null)
            {
                
                for (int i = 1; i < calcMoves[piece].Count; i++)
                {
                    Console.WriteLine("Trying to remove click 2222");
                    Console.WriteLine(i);
                    int endRow = calcMoves[piece][i][0];
                    int endCol = calcMoves[piece][i][1];
                    Console.WriteLine($"{endRow} {endCol}");
                    AllButtons[endRow, endCol].Click -= movePiece;
                    AllButtons[endRow, endCol].Click += button_Click;

                }

                /*ClearEventHandlers(AllButtons[endRow,endCol], "Click");*/
            }
        }

    }
}
