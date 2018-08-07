/***************************************************************
 * File: Board.cs
 * Created By: Justin Grindal		Date: 27 June, 2013
 * Description: The main chess board. Board contain the chess cell
 * which will contain the chess pieces. Board also contains the methods
 * to get and set the user moves.
 ***************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace ChessLibrary
{
	/// <summary>
	/// he main chess board. Board contain the chess cell
	/// which will contain the chess pieces. Board also contains the methods
	/// to get and set the user moves.
	/// </summary>
    [Serializable]
	public class Board
	{
		private Side m_WhiteSide, m_BlackSide;	// Chess board site object 
		private Cells m_cells;	// collection of cells in the board

		public Board()
		{
            m_WhiteSide = new Side(Side.SideType.White);	// Makde white side
            m_BlackSide = new Side(Side.SideType.Black);	// Makde white side

			m_cells = new Cells();					// Initialize the chess cells collection
		}

		// Initialize the chess board and place piece on thier initial positions
		public void Init(string mode = "Normal")
		{
			m_cells.Clear();		// Remove any existing chess cells

			// Build the 64 chess board cells
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					m_cells.Add(new Cell(row,col));	// Initialize and add the new chess cell
				}

            switch (mode)
            {
                case "Chess960":
                    InitChess960();
                    break;
                default:
                    InitNormal();
                    break;
            }      
		}

        private void PlaceBlackRooksAndKing960(Random rand, List<int> openPositions, List<Chess960SetupCell> setCells)
        {
            //Find first rook pos
            int randNum = rand.Next(8);

            m_cells[IntToChessStr(randNum) + "1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            int rookOnePos = randNum;
            openPositions.Remove(rookOnePos);
            setCells.Add(new Chess960SetupCell(rookOnePos, 8, Piece.PieceType.Rook));

            do
            {
                randNum = rand.Next(8);
            } while (Math.Abs(rookOnePos - randNum) < 2);

            int rookTwoPos = randNum;
            m_cells[IntToChessStr(rookTwoPos) + "1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            openPositions.Remove(rookTwoPos);
            setCells.Add(new Chess960SetupCell(rookTwoPos, 8, Piece.PieceType.Rook));

            if (rookTwoPos > rookOnePos)
            {
                randNum = rand.Next(rookOnePos + 1, rookTwoPos);
            }
            else
            {
                randNum = rand.Next(rookTwoPos + 1, rookOnePos);
            }

            int kingPos = randNum;
            m_cells[IntToChessStr(kingPos) + "1"].piece = new Piece(Piece.PieceType.King, m_BlackSide);
            openPositions.Remove(kingPos);
            setCells.Add(new Chess960SetupCell(kingPos, 8, Piece.PieceType.King));
        }

        private void PlaceBlackBishops960(Random rand, List<int> openPositions, List<Chess960SetupCell> setCells)
        {
            int randNum = openPositions[rand.Next(openPositions.Count)];

            int bishopOnePos = randNum;
            m_cells[IntToChessStr(bishopOnePos) + "1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            openPositions.Remove(bishopOnePos);
            setCells.Add(new Chess960SetupCell(bishopOnePos, 8, Piece.PieceType.Bishop));

            int bishopOneMod = ((bishopOnePos + 1) % 2);

            do
            {
                randNum = openPositions[rand.Next(openPositions.Count)];
            } while (bishopOneMod == ((randNum + 1) % 2));

            int bishopTwoPos = randNum;
            m_cells[IntToChessStr(bishopTwoPos) + "1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            openPositions.Remove(bishopTwoPos);
            setCells.Add(new Chess960SetupCell(bishopTwoPos, 8, Piece.PieceType.Bishop));

        }

        private void PlaceOtherBlackPiecesChess960(Random rand, List<int> openPositions, List<Chess960SetupCell> setCells)
        {

            int randNum = openPositions[rand.Next(openPositions.Count)];
            m_cells[IntToChessStr(randNum) + "1"].piece = new Piece(Piece.PieceType.Queen, m_BlackSide);
            openPositions.Remove(randNum);
            setCells.Add(new Chess960SetupCell(randNum, 8, Piece.PieceType.Queen));
            for (int j = 0; j < 2; j++)
            {
                randNum = openPositions[rand.Next(openPositions.Count)];
                m_cells[IntToChessStr(randNum) + "1"].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
                openPositions.Remove(randNum);
                setCells.Add(new Chess960SetupCell(randNum, 8, Piece.PieceType.Knight));
            }

        }

        private void PlaceWhiteSideChess960(List<Chess960SetupCell> setCells)
        {
            for (int j = 0; j < setCells.Count; j++)
            {
                m_cells[setCells[j].pos].piece = new Piece(setCells[j].type, m_WhiteSide);
            }
        }

        private string IntToChessStr(int num)
        {
            return ((char)('a' + num)).ToString();
        }

        public struct Chess960SetupCell
        {
            public string pos;
            public Piece.PieceType type;

            public Chess960SetupCell(int p, int row, Piece.PieceType t)
            {
                pos = ((char)('a' + p)).ToString() + row.ToString();
                type = t;
            }
        }

        private void InitChess960()
        {
            Random rand = new Random();
            List<int> openPositions = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
            List<Chess960SetupCell> setCells = new List<Chess960SetupCell>();

            PlaceBlackRooksAndKing960(rand, openPositions, setCells);
            PlaceBlackBishops960(rand, openPositions, setCells);
            PlaceOtherBlackPiecesChess960(rand, openPositions, setCells);
            PlaceWhiteSideChess960(setCells);

            // Now setup the pawns for black side
            for (int col = 1; col <= 8; col++)
                m_cells[2, col].piece = new Piece(Piece.PieceType.Pawn, m_BlackSide);

            // Now setup the pawns for white side
            for (int col = 1; col <= 8; col++)
                m_cells[7, col].piece = new Piece(Piece.PieceType.Pawn, m_WhiteSide);
        }

        private void InitNormal()
        {
            // Now setup the board for black side
            m_cells["a1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            m_cells["h1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            m_cells["b1"].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
            m_cells["g1"].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
            m_cells["c1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            m_cells["f1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            m_cells["e1"].piece = new Piece(Piece.PieceType.King, m_BlackSide);
            m_cells["d1"].piece = new Piece(Piece.PieceType.Queen, m_BlackSide);
            for (int col = 1; col <= 8; col++)
                m_cells[2, col].piece = new Piece(Piece.PieceType.Pawn, m_BlackSide);

            // Now setup the board for white side
            m_cells["a8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
            m_cells["h8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
            m_cells["b8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
            m_cells["g8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
            m_cells["c8"].piece = new Piece(Piece.PieceType.Bishop, m_WhiteSide);
            m_cells["f8"].piece = new Piece(Piece.PieceType.Bishop, m_WhiteSide);
            m_cells["e8"].piece = new Piece(Piece.PieceType.King, m_WhiteSide);
            m_cells["d8"].piece = new Piece(Piece.PieceType.Queen, m_WhiteSide);
            for (int col = 1; col <= 8; col++)
                m_cells[7, col].piece = new Piece(Piece.PieceType.Pawn, m_WhiteSide);
        }

		// get the new item by rew and column
		public Cell this[int row, int col]
		{
			get
			{
				return m_cells[row, col];
			}
		}

		// get the new item by string location
		public Cell this[string strloc]
		{
			get
			{
				return m_cells[strloc];	
			}
		}

		// get the chess cell by given cell
		public Cell this[Cell cellobj]
		{
			get
			{
				return m_cells[cellobj.ToString()];	
			}
		}

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlBoard = xmlDoc.CreateElement("Board");

            // Append game state attributes
            xmlBoard.AppendChild(m_WhiteSide.XmlSerialize(xmlDoc));
            xmlBoard.AppendChild(m_BlackSide.XmlSerialize(xmlDoc));

            xmlBoard.AppendChild(m_cells.XmlSerialize(xmlDoc));

            // Return this as String
            return xmlBoard;
        }

        /// <summary>
        /// DeSerialize the Board object from XML String
        /// </summary>
        /// <returns>XML containing the Board object state XML</returns>
        public void XmlDeserialize(XmlNode xmlBoard)
        {
            // Deserialize the Sides XML
            XmlNode side = XMLHelper.GetFirstNodeByName(xmlBoard, "Side");

            // Deserialize the XML nodes
            m_WhiteSide.XmlDeserialize(side);
            m_BlackSide.XmlDeserialize(side.NextSibling);

            // Deserialize the Cells
            XmlNode xmlCells = XMLHelper.GetFirstNodeByName(xmlBoard, "Cells");
            m_cells.XmlDeserialize(xmlCells);
        }

		// get all the cell locations on the chess board
		public ArrayList GetAllCells()
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					CellNames.Add(this[row,col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// get all the cell containg pieces of given side
        public ArrayList GetSideCell(Side.SideType PlayerSide)
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					// check and add the current type cell
					if (this[row,col].piece!=null && !this[row,col].IsEmpty() && this[row,col].piece.Side.type == PlayerSide)
						CellNames.Add(this[row,col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// Returns the cell on the top of the given cell
		public Cell TopCell(Cell cell)
		{
			return this[cell.row-1, cell.col];
		}

		// Returns the cell on the left of the given cell
		public Cell LeftCell(Cell cell)
		{
			return this[cell.row, cell.col-1];
		}

		// Returns the cell on the right of the given cell
		public Cell RightCell(Cell cell)
		{
			return this[cell.row, cell.col+1];
		}

		// Returns the cell on the bottom of the given cell
		public Cell BottomCell(Cell cell)
		{
			return this[cell.row+1, cell.col];
		}

		// Returns the cell on the top-left of the current cell
		public Cell TopLeftCell(Cell cell)
		{
			return this[cell.row-1, cell.col-1];
		}

		// Returns the cell on the top-right of the current cell
		public Cell TopRightCell(Cell cell)
		{
			return this[cell.row-1, cell.col+1];
		}

		// Returns the cell on the bottom-left of the current cell
		public Cell BottomLeftCell(Cell cell)
		{
			return this[cell.row+1, cell.col-1];
		}

		// Returns the cell on the bottom-right of the current cell
		public Cell BottomRightCell(Cell cell)
		{
			return this[cell.row+1, cell.col+1];
		}
	}
}
