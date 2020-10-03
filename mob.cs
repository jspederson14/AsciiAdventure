using System;
using System.Collections.Generic;

namespace asciiadventure {
    public class Mob : MovingGameObject {
        public Mob(int row, int col, Screen screen) : base(row, col, "#", screen) {}
        public int[] moveTo(int endR, int endC, bool light, List<Tuple<int, int>> moves){
            int[] end = new int[2];
            //moves mob towards the light if it exists
            if(light){
                int deltaRow;
                int deltaCol;
                //which direction on the row to move
                if(Row>player.Row)
                    deltaRow--;
                else if(Row<player.Row)
                    deltaRow++;
                //which direction on the col to move
                if(Col>player.Col)
                    deltaCol--;
                else if(Col<player.Col)
                    deltaCol++;
            }
            //moves mob randomly
            else
                var (deltaRow, deltaCol) = moves[random.Next(moves.Count)];
            end[0]=deltaRow;
            end[1]=deltaCol;
        }
    }
}