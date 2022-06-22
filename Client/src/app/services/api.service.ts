import { Injectable } from "@angular/core"
import { baseUrl } from "src/constants"
import { AuthService } from "./auth.service"
import { NotificationService } from "./notification.service"

export class MyReq {
	url: string = ""
	model: any = {}
	method: string = "POST"
	success: (any) => void = () => {}
	notify: boolean = true
	isAdmin: boolean = false
	error: (any) => void = () => {}
	name: string
	useToken: boolean = true

	constructor(name: string) {
		this.name = name
	}
}

@Injectable({
	providedIn: "root",
})
export class ApiService {
	constructor(private notif: NotificationService, private auth: AuthService) {}

	getLoggedUser(admin: boolean) {
		let user
		if (admin) {
			if (localStorage.qrha == null) throw { code: "T", message: "Error de token." }
			user = JSON.parse(localStorage.qrha)
		} else {
			if (localStorage.qrh == null) throw { code: "T", message: "Error de token." }
			user = JSON.parse(localStorage.qrh)
		}
		return user
	}

	async request(req: MyReq) {
		try {
			if (req.notify) this.notif.showLoading()

			let token = ""
			if (req.useToken) {
				let user = this.getLoggedUser(req.isAdmin)
				token = user.token
			}

			const url = baseUrl + req.url
			const rawResponse = await fetch(url, {
				method: req.method,
				headers: {
					Accept: "application/json",
					"Content-Type": "application/json",
					token: token,
				},
				body: JSON.stringify(req.model),
			})
			const res = await rawResponse.json()

			//console.log({ name: req.name, res })
			this.auth.verify(res, req.isAdmin)
			if (typeof res.status !== "boolean") throw res
			if (!res.status) throw { message: res.message }
			req.success(res)
		} catch (e) {
			console.log({ name: req.name, error: e })
			if (e.code == "T") this.auth.logout(req.isAdmin)
			if (e.message == "Failed to fetch") e.message = "No se pudo conectar con el servidor"
			if (req.notify) this.notif.error(e.message)
			req.error(e)
		} finally {
			if (req.notify) this.notif.hideLoading()
		}
	}
}
