using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schneider
{
    class FilterIp
    {
        /// <summary>
        /// Check if the ip is correct and return ip value corrected(without blanks spaces)
        /// </summary>
        /// <returns>True if the ip is correct, but false</returns>
        public bool CheckIp(string ip, out string ipCorrected)
        {
            var noSpacesIp = ip.Trim(); //Delete the possible blank spaces
            var arraySplitIp = noSpacesIp.Split('.');//split the ip in elements that has to be separated by a point 
            int [] arrayNumIp = new int[4];//Array to save the numerical elements of the ip
            var countElementsIp = 0;//Variable to count the elements of the ip
            bool ipIsCorrect = false;//Variable that the method returns, it is true when the ip is correct

            //Check if there are four elements
            for (var i = 0; i < arraySplitIp.Length; i++)
            {
                countElementsIp++;
            }
            //if the number of elements are four
            if(countElementsIp == 4)
            {
                //Check if all of the elements are numbers
                for (var i = 0; i < arraySplitIp.Length; i++)
                {
                    ipIsCorrect = int.TryParse(arraySplitIp[i], out arrayNumIp[i]);
                    if (ipIsCorrect)
                    {
                        //Check if any element is up or down the valid range
                        if (arrayNumIp[i] < 0 || arrayNumIp[i] > 255)
                        {
                            ipIsCorrect = false;
                        }

                    }
                }
            }
            if (ipIsCorrect)
            {
                //We save every numerical element of the ip separated by a point
                ipCorrected = arrayNumIp[0] +"."+ arrayNumIp[1] +"."+ arrayNumIp[2] +"."+ arrayNumIp[3];
            }
            else
            {
                ipCorrected = ip;
            }
            return ipIsCorrect;
        }
    }
}
