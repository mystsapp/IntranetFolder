using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IntranetFolder.Models;
using Data.Repository;
using GleamTech.FileUltimate.AspNet.UI;
using Microsoft.AspNetCore.Http;
using Data.Models;
using Data.Utilities;

namespace IntranetFolder.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public HomeViewModel HomeVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            HomeVM = new HomeViewModel()
            {
                
                FolderUser = new Data.Models.FolderUser(),
                FileManager = new FileManager()
            };
        }
        
        public async Task<IActionResult> Index()
        {
            User user = HttpContext.Session.Gets<User>("loginUser").FirstOrDefault();
            HomeVM.FolderUsers = await _unitOfWork.folderUserReprository.FindAsync(x => x.UserId == user.Username);
            return View(HomeVM);
        }
        
        public IActionResult ExploreUrl(long folderUserId)
        {
            
            HomeVM.FolderUser = _unitOfWork.folderUserReprository.GetById(folderUserId);

            var fileManager = new FileManager
            {
                Width = 800,
                Height = 600,
                Resizable = true,

            };
            
            var rootFolder = new FileManagerRootFolder
            {

                Name = "A Root Folder",
                //Location = @"E:\softs"
                Location = HomeVM.FolderUser.Path
                //Location = @"\\192.168.4.153\Ghosts"
            };

            if(HomeVM.FolderUser.Upload == true)
            {
                rootFolder.AccessControls.Add(new FileManagerAccessControl
                {
                    Path = @"\",
                    AllowedPermissions = FileManagerPermissions.Full
                });

            }
            else
            {
                rootFolder.AccessControls.Add(new FileManagerAccessControl
                {
                    Path = @"\",
                    AllowedPermissions = FileManagerPermissions.ReadOnly
                });

            }

            fileManager.RootFolders.Add(rootFolder);
            fileManager.ShowRibbon = true;

            HomeVM.FileManager = fileManager;
            return View(HomeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
