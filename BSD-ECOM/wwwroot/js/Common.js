function ReloadPage(controllerName, ActionName) {
    window.location = "/" + controllerName + "/" + ActionName;
}
function ReloadPageWithAreas(areasname,controllerName, ActionName) {
    window.location = "/" + areasname+"/" + controllerName + "/" + ActionName;
}

function ReloadPageWithRoute(routename) {
    window.location = "/" + routename ;
}

function ReloadPageWithRouteandId(routename,id) {
    window.location = "/" + routename+"/"+id;
}