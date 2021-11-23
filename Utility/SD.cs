using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Utility
{
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Viewer = "Viewer";
        public const string Role_Viewer_Id = "6f82bd46-0fec-4a03-b81a-8171ac974aa8";


        public const string Role_GL = "Group Leader";
        public const string Role_BUM = "Business Unit Leader";
        public const string Role_PX = "Project Executive";
        public const string Role_Coordinator = "Project Coordinator";

        public const string Link_KeyPlayer = "https://cloudcmic.pepperconstruction.com/cmicprod/SysLaunchPopup/Index.jsp?objType=OMOPPO&objOraseq=";

        public const string Cache_ProjectUser = "Cache_ProjectUser";
        public const string Session_Company_Key = "_SessionKey_Company";

        public const string ResumeRootUrl = @"/illinois/marketingresumes/IL/";
        // Sto Procedure
        public const string Stoproc_GetEmpExperience = "GetEmpExperience";
        public const string Stoproc_GetEmpEducation = "GetEmpEducation";
        public const string Stoproc_GetEmpCertification = "GetEmpCertification";
        public const string Stoproc_GetEmpMarketExperience = "GetEmpMarketExperience";
        public const string Stoproc_GetProjectNote = "GetEmployeeProjectNote";
        public const string Stoproc_UpdateProjectNote = "UpdateProjectNote"; // Update Note
        public const string Stoproc_InsertProjectNote = "InsertProjectNote"; // Insert Note
        public const string Stoproc_DeleteProjectNote = "DeleteProjectNote"; // Delete Note
        public const string Stocproc_GetNoteDetail = "GetEmployeeProjectNoteDetail";





    }
}
