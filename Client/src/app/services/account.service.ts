import { Injectable } from "@angular/core"
import { ACCOUNT_HOTEL, LOGIN_ADMIN, LOGIN_CLIENT } from "../api/endpoints"
import { LoginAdminModel, LoginModel } from "../models/account"
import { ApiService, MyReq } from "./api.service"
import { NotificationService } from "./notification.service"

@Injectable({
	providedIn: "root",
})
export class AccountService {
	constructor(private notif: NotificationService, private api: ApiService) {}

	async login(model: LoginModel, callback: () => void) {
		if (model.LastName.trim() == "") {
			this.notif.error("Debe ingresar el Apellido.")
			return
		}
		if (model.RoomCode.trim() == "") {
			this.notif.error("Debe ingresar el Número de habitación.")
			return
		}
		if (!model.Conditions) {
			this.notif.error("Debe aceptar los términos y condiciones.")
			return
		}
		let req = new MyReq("login client")
		req.model = model
		req.url = LOGIN_CLIENT
		req.useToken = false
		req.success = res => {
			localStorage.qrh = JSON.stringify(res.data)
			callback()
		}
		await this.api.request(req)
	}

	async loginAdmin(model: LoginAdminModel, callback: () => void) {
		if (model.UserName.trim() == "") {
			this.notif.error("Debe ingresar su usuario.")
			return
		}
		if (model.Password.trim() == "") {
			this.notif.error("Debe ingresar su password.")
			return
		}
		let req = new MyReq("login admin")
		req.url = LOGIN_ADMIN
		req.model = model
		req.useToken = false
		req.success = res => {
			localStorage.qrha = JSON.stringify(res.data)
			callback()
		}
		await this.api.request(req)
	}

	async hotel(urlparam: string) {
		let result = null
		let req = new MyReq("get hotel data from url")
		req.url = ACCOUNT_HOTEL
		req.model = { hotelCode: urlparam }
		req.useToken = false
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}
}
