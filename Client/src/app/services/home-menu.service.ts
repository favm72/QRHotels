import { Injectable } from "@angular/core"
import { HOMEMENU_CREATE, HOMEMENU_EDIT, HOMEMENU_LIST, HOMEMENU_LISTADMIN } from "../api/endpoints"
import { ApiService, MyReq } from "./api.service"

export const HomeMenuType = {
	slider: { id: "slider", label: "Ofertas" },
	restaurants: { id: "restaurants", label: "Restaurantes" },
	services: { id: "services", label: "Servicios" },
	preCheck: { id: "preCheck", label: "Pre CheckOut" },
	lateCheck: { id: "lateCheck", label: "Late CheckOut" },
	experience: { id: "experience", label: "Experiencias" },
	store: { id: "store", label: "Tiendita" },
	sama: { id: "sama", label: "Sama2Go" },
	ammenities: { id: "ammenities", label: "Ammenities" },
	informative: { id: "informative", label: "Página Informativa" },
	cleaning: { id: "cleaning", label: "Limpieza" },
	link: { id: "link", label: "Enlace externo" },
}

@Injectable({
	providedIn: "root",
})
export class HomeMenuService {
	constructor(private api: ApiService) {}

	async list() {
		let menu = []
		let req = new MyReq("list home menu")
		req.url = HOMEMENU_LIST
		req.success = res => {
			menu = res.data
		}
		await this.api.request(req)
		return menu
	}

	async listAdmin(hotelCode: string) {
		let menu = []
		let req = new MyReq("list home menu admin")
		req.url = HOMEMENU_LISTADMIN
		req.model = { hotelCode }
		req.isAdmin = true
		req.success = res => {
			menu = res.data
		}
		await this.api.request(req)
		return menu
	}

	async create(model: any) {
		let result = false
		let req = new MyReq("create home menu")
		req.url = HOMEMENU_CREATE
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async edit(model: any) {
		let result = false
		let req = new MyReq("edit home menu")
		req.url = HOMEMENU_EDIT
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}
}
