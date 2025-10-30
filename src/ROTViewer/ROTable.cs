using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

namespace ROTViewer
{
    /// <summary>
    /// redefinition d'un type de LINQ pour les besoins du framework 2.0. A supprimer dès que l'on passe au framework 3.
    /// </summary>
    /// <typeparam name="Tin">premier paramètre</typeparam>
    /// <typeparam name="Tret">type de retour</typeparam>
    /// <param name="p1"></param>
    /// <returns></returns>
    public delegate Tret Func2<Tin, Tret>(Tin p1);

    /// <summary>
    /// représente la table ROT (running object table) et permet d'enumérer ses élements.
    /// </summary>
    public static class ROTable
    {
        /// <summary>
        /// la collection complète des éléments de la ROT.
        /// </summary>
        public static IEnumerable<ROTMoniker> Monikers
        {
            get
            {
                Ole32.CreateBindCtx(0, out IBindCtx bc);

                bc.GetRunningObjectTable(out IRunningObjectTable rot);
                Marshal.ReleaseComObject(bc);

                rot.EnumRunning(out IEnumMoniker comenum);
                Marshal.ReleaseComObject(rot);

                try
                {
                    List<ROTMoniker> list = new List<ROTMoniker>();
                    comenum.Reset();
                    int celt = 1;
                    IMoniker[] monikers = new IMoniker[1];
                    IntPtr pfetched = new IntPtr();
                    while (comenum.Next(celt, monikers, pfetched) == 0)
                    {
                        list.Add(new ROTMoniker(monikers[0], null));
                    }
                    list = (from data in list
                            orderby data.Name
                            select data).ToList<ROTMoniker>();
                    foreach (ROTMoniker moniker in list)
                        yield return moniker;

                    //comenum.Reset();
                    //int celt = 1;
                    //IMoniker[] monikers = new IMoniker[1];
                    //IntPtr pfetched = new IntPtr();
                    //while (comenum.Next(celt, monikers, pfetched) == 0)
                    //{
                    //    yield return new ROTMoniker(monikers[0], null);
                    //}
                }
                finally
                {
                    Marshal.ReleaseComObject(comenum);
                }
            }
        }
        /// <summary>
        /// returns the first moniker matching the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static ROTMoniker FindFirstMoniker(Func2<ROTMoniker, bool> predicate)
        {
            foreach (ROTMoniker moniker in Monikers)
            {
                if (predicate(moniker))
                {
                    return moniker;
                }
            }
            return null;
        }

        /// <summary>
        /// find all monikers matching the predicate
        /// </summary>
        /// <param name="predicate">a lambda expression like x=> x.Name.StartsWith("Excel.Application::") </param>
        /// <returns></returns>
        public static IEnumerable<ROTMoniker> FindMonikers(Func2<ROTMoniker, bool> predicate)
        {
            foreach (ROTMoniker moniker in Monikers)
            {
                if (predicate(moniker))
                {
                    yield return moniker;
                }
            }
        }
    }
}
