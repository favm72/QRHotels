import { Injectable } from "@angular/core"
import { DIRECTORY_CREATE, DIRECTORY_EDIT, DIRECTORY_LIST, DIRECTORY_LISTADMIN, DIRECTORY_UPSERTHEAD } from "../api/endpoints"
import { ApiService, MyReq } from "./api.service"

@Injectable({
	providedIn: "root",
})
export class DirectoryService {
	constructor(private api: ApiService) {}

	async list(): Promise<{ head: any; detail: any[] }> {
		let result = { head: {}, detail: [] }
		let req = new MyReq("list directories")
		req.url = DIRECTORY_LIST
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async listAdmin(hotelCode: string): Promise<{ head: any; detail: any[] }> {
		let result = { head: {}, detail: [] }
		let req = new MyReq("list directories admin")
		req.url = DIRECTORY_LISTADMIN
		req.model = { hotelCode }
		req.isAdmin = true
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async upsertHead(model: any) {
		let result = false
		let req = new MyReq("usert directory")
		req.url = DIRECTORY_UPSERTHEAD
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async create(model: any) {
		let result = false
		let req = new MyReq("create directory item")
		req.url = DIRECTORY_CREATE
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
		let req = new MyReq("edit directory item")
		req.url = DIRECTORY_EDIT
		req.isAdmin = true
		req.model = model
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}
}
