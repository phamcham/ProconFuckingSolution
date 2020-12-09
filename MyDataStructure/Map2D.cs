using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconFuckingSolution.MyDataStructure {

	public class Map2D<T> {
		private T[,] arr;
		private int offset;

		public T this[int row, int column] {
			get { return arr[row + offset, column + offset]; }
			set { arr[row + offset, column + offset] = value; }
		}

		public Map2D(int size) {
			arr = new T[3 * size + 5, 3 * size + 5];
			offset = size;
		}

		public Map2D<T> Clone() {
			Map2D<T> tmp = new Map2D<T>(offset);
			for (int i = 1; i <= 2 * offset + 3; i++) {
				for (int j = 1; j <= 2 * offset + 3; j++) {
					tmp.arr[i, j] = arr[i, j];
				}
			}
			return tmp;
		}
	}
}