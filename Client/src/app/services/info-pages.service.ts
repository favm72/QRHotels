import { Injectable } from "@angular/core"
import {
	INFOPAGE_CREATE,
	INFOPAGE_DETAIL_CREATE,
	INFOPAGE_DETAIL_EDIT,
	INFOPAGE_DETAIL_LIST,
	INFOPAGE_DETAIL_LISTADMIN,
	INFOPAGE_EDIT,
	INFOPAGE_LIST,
} from "../api/endpoints"
import { ApiService, MyReq } from "./api.service"

@Injectable({
	providedIn: "root",
})
export class InfoPagesService {
	currentPageId: number = 0
	constructor(private api: ApiService) {}

	async listDetail(id: number) {
		let result = []
		let req = new MyReq("info page list detail")
		req.url = INFOPAGE_DETAIL_LIST
		req.model = { idInfoPage: id }
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async listDetailAdmin(id: number) {
		let result = []
		let req = new MyReq("info page list detail admin")
		req.url = INFOPAGE_DETAIL_LISTADMIN
		req.model = { idInfoPage: id }
		req.isAdmin = true
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async listHead(hotelCode: string) {
		let result = []
		let req = new MyReq("info page list head admin")
		req.url = INFOPAGE_LIST
		req.model = { hotelCode }
		req.isAdmin = true
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async createDetail(model: any) {
		let result = false
		let req = new MyReq("create info page detail")
		req.url = INFOPAGE_DETAIL_CREATE
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async editDetail(model: any) {
		let result = false
		let req = new MyReq("edit info page detail")
		req.url = INFOPAGE_DETAIL_EDIT
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async createHead(model: any) {
		let result = false
		let req = new MyReq("create info page head")
		req.url = INFOPAGE_CREATE
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async editHead(model: any) {
		let result = false
		let req = new MyReq("edit info page head")
		req.url = INFOPAGE_EDIT
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}
}
