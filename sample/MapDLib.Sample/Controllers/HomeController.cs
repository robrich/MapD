namespace MapDLib.Sample.Controllers {
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
	using MapDLib.Sample.Models;
	using MapDLib.Sample.Some_Other_Project;

	public class HomeController : Controller {

		public ActionResult Index() {
			return View();
		}

		public ActionResult SimpleType() {
			// Fetched from a business class or a data layer
			SomeClass1 c = new SomeClass1 {
				Property1 = "the property 1",
				Property2 = "the other property 2"
			};

			SomeClass1ViewModel model = MapD.Copy<SomeClass1, SomeClass1ViewModel>( c ); // Copy simple type
			return View( model );
		}

		public ActionResult ComplexType() {
			// Fetched from a business class or a data layer
			List<SomeListClass1> list = new List<SomeListClass1>() {
				new SomeListClass1 {
					SomeId = 1,
					Name = "some1",
					OriginDate = DateTime.Now,
					Interested = true
				},
				new SomeListClass1 {
					SomeId = 2,
					Name = "some2",
					OriginDate = DateTime.Now.AddDays( 2 ),
					Interested = false
				}
			};

			List<SomeListClass1ViewModel> model = MapD.Copy<List<SomeListClass1>, List<SomeListClass1ViewModel>>( list ); // Copy list
			return View( model );
		}

		[HttpPost]
		public ActionResult ComplexType(List<SomeListClass1ViewModel> model ) {
			if ( ModelState.IsValid ) {
				List<SomeListClass1> entities = null; // Pulled from some business class
				List<PropertyChangedResults> changes = MapD.CopyBack<List<SomeListClass1>, List<SomeListClass1ViewModel>>( model, ref entities );
				if ( changes.Count > 0 ) {
					//DataLayer.Save( entities );
				} else {
					// Nothing changed
				}
				return RedirectToAction( "Index" );
			}
			// Fix your errors
			return View( model );
		}

	}
}