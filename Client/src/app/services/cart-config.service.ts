import { Injectable } from "@angular/core"
import { CART_CONFIG_ADD, CART_CONFIG_EDIT, CART_CONFIG_LIST } from "../api/endpoints"
import { ApiService, MyReq } from "./api.service"

@Injectable({
	providedIn: "root",
})
export class CartConfigService {
	constructor(private api: ApiService) {}

	async list() {
		let data = []
		let req = new MyReq("list cart config")
		req.url = CART_CONFIG_LIST
		req.model = {}
		req.isAdmin = true
		req.success = res => {
			data = res.data
		}
		await this.api.request(req)
		return data
	}

	async add(cartConfig, success) {
		let req = new MyReq("add cart config")
		req.url = CART_CONFIG_ADD
		req.model = cartConfig
		req.isAdmin = true
		req.success = success
		await this.api.request(req)
	}

	async edit(cartConfig, success) {
		let req = new MyReq("edit cart config")
		req.url = CART_CONFIG_EDIT
		req.model = cartConfig
		req.isAdmin = true
		req.success = success
		await this.api.request(req)
	}
}
