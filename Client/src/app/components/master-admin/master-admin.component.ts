import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { faBars } from "@fortawesome/free-solid-svg-icons"

@Component({
	selector: "app-master-admin",
	templateUrl: "./master-admin.component.html",
	styleUrls: ["./master-admin.component.css"],
})
export class MasterAdminComponent implements OnInit {
	fullname = ""
	menuicon = faBars
	menuActive = false

	constructor(private router: Router) {}

	ngOnInit(): void {
		let user = JSON.parse(localStorage.qrha)
		this.fullname = user.name
	}

	logout() {
		localStorage.removeItem("qrha")
		this.router.navigate(["/loginadmin"])
	}

	showMenu() {
		this.menuActive = !this.menuActive
	}
}
