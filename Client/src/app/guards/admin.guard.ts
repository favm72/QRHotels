import { Injectable } from "@angular/core"
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from "@angular/router"
import { Observable } from "rxjs"

@Injectable({
	providedIn: "root",
})
export class AdminGuard implements CanActivate, CanActivateChild {
	constructor(private router: Router) {}

	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (localStorage.qrha != null) {
			return true
		} else {
			this.router.navigate(["/loginadmin"])
			return false
		}
	}
	canActivateChild(
		childRoute: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		if (localStorage.qrha != null) {
			return true
		} else {
			this.router.navigate(["/loginadmin"])
			return false
		}
	}
}
