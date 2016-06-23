using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtSys
{
  internal sealed class MICollection
  {
    static internal Int32 MATRX_LENGHT { get { return 500; } }


    private Int32 m_current_index;

    private MatrixItem[] m_list = new MatrixItem[MATRX_LENGHT];

    private UInt16 request_id;

    internal UInt16 REQUEST_ID
    {
      get { return request_id; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="newindex">new index for inner INDEX</param>
    /// <exception cref="System.IndexOutOfRangeException">When index out of range</exception>"
    internal void setActiveIndex(Int32 newindex)
    {
      if (!IndexInRange(newindex))
      {
        throw new IndexOutOfRangeException(" New index out of MI list");
      }
      m_current_index = newindex;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="index_seek">requested ID</param>
    /// <returns>founded item</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">if not founded requested ID</exception>
    internal MatrixItem getItemByRequestId(UInt16 index_seek)
    {
      Int32 seek_position = 0;
      foreach (MatrixItem m in m_list)
      {
        if (m != null && m.id == index_seek)
        {
          m_current_index = seek_position;
          return m;
        }
        seek_position++;
      }
      throw new ArgumentOutOfRangeException(" MItem not founded");
    }


    /// <summary>
    /// extract MItem by inner index
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException">when requested id havn't correct MItem</exception>
    internal MatrixItem getItemByCurrentIndex()
    {
      if (m_list[m_current_index] == null)
        throw new NullReferenceException(" MItem is null");
      return m_list[m_current_index];
    }


    public MatrixItem getItemByNumInList(Int32 index_seek)
    {
      setActiveIndex(index_seek);
      return getItemByCurrentIndex();
    }

    internal void StepAheadIndex()
    {
      m_current_index++;
    }


    internal void LoadNewItemInCurrentIndex(MatrixItem m)
    {
      mLoadNewItem(m, m_current_index);
    }

    internal void UpdateRequestID()
    {
      request_id = getItemByCurrentIndex().id;
    }

    /// <summary>
    /// Update dedicated list item. 
    /// If m - NULL or indx not in range -> NullReferenceException
    /// </summary>
    /// <param name="m">new MI</param>
    /// <param name="indx">appropriated index</param>
    private void mLoadNewItem(MatrixItem m, Int32 indx)
    {
      if (m == null || !IndexInRange(indx))
        throw new NullReferenceException();

      m_list[indx] = m;
    }


    private static bool IndexInRange(Int32 index)
    {
      /* if index out of range return false */
      return (((index < 0) || (index >= MATRX_LENGHT)) ? (false) : (true));
    }

  };

}
