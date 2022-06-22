import { Injectable } from "@angular/core"
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from "@angular/router"
import { Observable } from "rxjs"

@Injectable({
	providedIn: "root",
})
export class ClientGuard implements CanActivate, CanActivateChild {
	constructor(private router: Router) {}
	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (localStorage.qrh != null) {
			return true
		} else {
			this.router.navigate(["/login/CASA-MIRAFLORESPREMIUM"])
			return false
		}
	}
	canActivateChild(
		childRoute: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (localStorage.qrh != null) {
			return true
		} else {
			this.router.navigate(["/login/CASA-MIRAFLORESPREMIUM"])
			return false
		}
	}
}
