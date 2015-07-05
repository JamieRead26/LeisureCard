#Leisure Cards

The Leisure Card system consists of 2 web applications, one serves the API and the other serves the UI.  The UI is implemented using AngularJS and interacts with the API. 

In addition there is a .NET client library for the API here: https://github.com/ShiftkeySoftware/GRG.LeisureCards/tree/Integration/src/GRG.LeisureCards.API/GRG.LeisureCards.WebAPI.Client.

#Multi-Tenant Content

The Leisure Card system hosts multiple clients.  Each client has their own set of urn data, styles, images and content.  Currently all clients share a global set of offer data and page layouts.

The root of the UI web has a content folder, under which is a folder for each client, the name of which matches the configured client key, e.g.;

- Content
  - NPower
    - css (style sheets)
	- img (images) 
	- 241Category (241 category images)
	- html (custom content)
	  - terms (this folder name must match any view in the Views/Partials directory)
	    - terms.html (each file contains html that will be placed in the ViewBag with a key matching the file name, in this case "ViewBag.terms")
		- additional.html (multiple files in a single folder will results in multiple entries in the ViewBag when rendering that view., in the case "ViewBag.additional")

To reference the custom content simply put the following directive in the partial view:

 @Html.Raw(ViewBag.terms) //here terms is the name of the file in the content/html/{viewname}/terms.html file



