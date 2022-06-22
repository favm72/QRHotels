import { Injectable } from "@angular/core"
import { PRODUCT_ALLCATEGORIES, PRODUCT_CATEGORIES, PRODUCT_CREATE, PRODUCT_FINDBYID, PRODUCT_SEARCH, PRODUCT_UPDATE } from "../api/endpoints"
import { ApiService, MyReq } from "./api.service"
import { NotificationService } from "./notification.service"

@Injectable({
	providedIn: "root",
})
export class ProductService {
	constructor(private api: ApiService, private notif: NotificationService) {}

	async categories(model) {
		let categories = []
		let req = new MyReq("load categories")
		req.url = PRODUCT_CATEGORIES
		req.isAdmin = true
		req.model = model
		req.success = res => {
			categories = res.data
		}
		await this.api.request(req)
		return categories
	}

	async allcategories() {
		let categories = []
		let req = new MyReq("load all categories")
		req.url = PRODUCT_ALLCATEGORIES
		req.isAdmin = true
		req.success = res => {
			categories = res.data
		}
		await this.api.request(req)
		return categories
	}

	async search(filter) {
		let products = []
		let req = new MyReq("load products")
		req.url = PRODUCT_SEARCH
		req.isAdmin = true
		req.model = filter
		req.success = res => {
			products = res.data
		}
		await this.api.request(req)
		return products
	}

	async findbyid(product) {
		let result = null
		let req = new MyReq(`find product by id = ${product.id}`)
		req.url = PRODUCT_FINDBYID
		req.isAdmin = true
		req.model = product
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async create(product) {
		let result = null
		let req = new MyReq(`create product`)
		req.url = PRODUCT_CREATE
		req.isAdmin = true
		req.model = product
		req.success = res => {
			result = res.data
			this.notif.success("Registrado correctamente.")
		}
		await this.api.request(req)
		return result
	}

	async update(product) {
		let result = null
		let req = new MyReq(`update product id = ${product.id}`)
		req.url = PRODUCT_UPDATE
		req.isAdmin = true
		req.model = product
		req.success = res => {
			result = res.data
			this.notif.success("Actualizado correctamente.")
		}
		await this.api.request(req)
		return result
	}
}
