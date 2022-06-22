import { Component, OnDestroy, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { LoginAdminModel } from "src/app/models/account"
import { AccountService } from "src/app/services/account.service"

@Component({
	selector: "app-login-admin",
	templateUrl: "./login-admin.component.html",
	styleUrls: ["./login-admin.component.css"],
})
export class LoginAdminComponent implements OnInit {
	model: LoginAdminModel = new LoginAdminModel()

	constructor(private router: Router, private account: AccountService) {}

	async onLogin(): Promise<void> {
		await this.account.loginAdmin(this.model, () => this.goToMain())
	}

	goToMain() {
		this.router.navigate(["/admin/main"])
	}

	ngOnInit(): void {}
}
