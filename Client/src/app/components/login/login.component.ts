import { Component, OnDestroy, OnInit } from "@angular/core"
import { ActivatedRoute, Router } from "@angular/router"
import { LoginModel } from "./../../models/account"
import { AccountService } from "src/app/services/account.service"

@Component({
	selector: "app-login",
	templateUrl: "./login.component.html",
	styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit {
	hotel = null
	model: LoginModel = new LoginModel()

	constructor(private router: Router, private account: AccountService, private route: ActivatedRoute) {
		this.route.params.subscribe(params => {
			this.model.HotelCode = params["hotelCode"]
		})
	}

	async onLogin(): Promise<void> {
		await this.account.login(this.model, () => this.goToMain())
	}

	goToMain() {
		this.router.navigate(["/master/main"])
	}

	async ngOnInit(): Promise<void> {
		if (localStorage.qrh != null) {
			this.goToMain()
			return
		}
		this.hotel = await this.account.hotel(this.model.HotelCode)
	}
}
