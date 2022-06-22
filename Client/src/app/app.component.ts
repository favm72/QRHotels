import { Component, OnDestroy, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { Subscription } from "rxjs"
import { AuthService } from "./services/auth.service"

@Component({
	selector: "app-root",
	templateUrl: "./app.component.html",
	styleUrls: ["./app.component.css"],
})
export class AppComponent implements OnInit, OnDestroy {
	title = "qrhoteles"
	authSub: Subscription = null
	constructor(private auth: AuthService, private router: Router) {}
	ngOnDestroy(): void {
		if (this.authSub != null) this.authSub.unsubscribe()
	}
	ngOnInit(): void {
		this.authSub = this.auth.onLogout.subscribe(isadmin => {
			if (isadmin) {
				this.router.navigate(["/loginadmin"])
			} else {
				this.router.navigate(["/login/CASA-MIRAFLORESPREMIUM"])
			}
		})
	}
}
