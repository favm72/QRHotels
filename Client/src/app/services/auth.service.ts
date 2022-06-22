import { Injectable } from "@angular/core"
import { Subject } from "rxjs"

@Injectable({
	providedIn: "root",
})
export class AuthService {
	onLogout: Subject<boolean> = new Subject<boolean>()

	verify(res, admin: boolean) {
		if (!res.status && res.logout) {
			this.logout(admin)
		}
	}

	logout(admin: boolean) {
		if (admin) {
			localStorage.removeItem("qrha")
		} else {
			localStorage.removeItem("qrh")
			localStorage.removeItem("order")
			localStorage.removeItem("number")
			localStorage.removeItem("prefix")
		}
		this.onLogout.next(admin)
	}

	constructor() {}
}
