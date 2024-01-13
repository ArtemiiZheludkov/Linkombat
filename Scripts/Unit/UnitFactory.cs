using UnityEngine;

namespace PuzzlePaint
{
    public class UnitFactory
    {
        private Unit[] _units;
        
        private int _unitsInRow;
        private float _offsetX;
        private float _offsetZ;
        
        public UnitFactory(int unitsInRow, float offsetX, float offsetZ)
        {
            _unitsInRow = unitsInRow;

            if (_unitsInRow < 1)
                _unitsInRow = 1;

            _offsetX = offsetX;
            _offsetZ = offsetZ;
        }
        
        public Unit[] CreateUnits(Unit unit, int amount, Transform container)
        {
            _units = new Unit[amount];

            for (int i = 0; i < amount; i++)
            {
                _units[i] = GameObject.Instantiate(unit, container.position, Quaternion.identity, container);
                //units[i].Deactivate();
            }
            
            int rows = amount / _unitsInRow;
            
            if (rows < 1)
                rows = 1;

            float z = rows * _offsetZ;

            if (container.position.z < 0)
                z = -_offsetZ;
            
            FormationOfMainRows(amount, rows);
            FormationOfLastRow(amount, rows, z);
            
            return _units;
        }

        private void FormationOfMainRows(int amount, int rows)
        {
            int rowsAmount = _unitsInRow;
            
            if (amount < _unitsInRow)
                rowsAmount = amount;

            int forwardZ = 1;

            for (int i = 0; i < rows; i++)
            {
                float x = _offsetX / 2f;

                for (int j = 0; j < rowsAmount; j++)
                {
                    if (j % 2 == 0 && j > 0)
                        x += _offsetX;
                    
                    _units[rowsAmount * i + j].transform.localPosition = new Vector3(x, 0f, ((i * forwardZ) * _offsetZ));

                    x = -x;
                }
            }
        }

        private void FormationOfLastRow(int amount, int rows, float z)
        {
            int inLastRow = amount - rows * _unitsInRow;

            float x = _offsetX / 2f;

            for (int i = 0; i < inLastRow; i++)
            {
                if (i % 2 == 0 && i > 0)
                    x += _offsetX;

                _units[amount - i - 1].transform.localPosition = new Vector3(x, 0f, z);

                x = -x;
            }
        }
    }
}