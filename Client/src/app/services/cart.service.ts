import { Injectable } from "@angular/core"
import { ORDER_DISCOUNT, ORDER_SAVE, ORDER_VALIDATE_BREAKFAST, PRODUCT_CART_ENABLED, PRODUCT_LIST } from "../api/endpoints"
import { OrderMessageModel } from "../models/order"
import { ApiService, MyReq } from "./api.service"
import { InfoService } from "./info.service"
import { NotificationService } from "./notification.service"
import { SignalOrderService } from "./signal-order.service"

@Injectable({
	providedIn: "root",
})
export class CartService {
	provider: number = 0
	categories = []
	viewOnly: boolean = false
	hotelCode: string = ""
	order: any = {
		discount: 0,
		promoName: "",
		idPromo: null,
		products: [],
	}
	elements: number = 0
	product = null

	constructor(private notif: NotificationService, private api: ApiService, private signal: SignalOrderService, private info: InfoService) {}

	async loadProducts() {
		let req = new MyReq("load products")
		req.url = PRODUCT_LIST
		req.model = { IdProvider: this.provider }
		req.success = res => {
			this.categories = res.data
		}
		await this.api.request(req)
	}

	async addToCartBreakFast(callback) {
		this.addToCart(() => {})
		this.setOrder(() => {})
		let req = new MyReq("validate breakfast quantities")
		req.url = ORDER_VALIDATE_BREAKFAST
		req.model = this.order
		req.success = _ => callback()
		req.error = _ => this.removeFromCart(this.product, () => {})
		await this.api.request(req)
	}

	updateCart() {
		this.elements = 0
		let orderjson = localStorage.order
		if (orderjson != null) {
			this.order.products = JSON.parse(orderjson)
			for (const item of this.order.products) {
				for (const cat of this.categories) {
					for (const prod of cat.products) {
						if (prod.id == item.id) {
							prod.quantity = item.quantity
							prod.comment = item.comment
							this.elements += item.quantity
						}
					}
				}
			}
		}
	}

	removeFromCart(product, onError) {
		try {
			let orderjson = localStorage.order
			if (orderjson == null) throw { message: "No ha agregado elementos al carrito" }
			let current = JSON.parse(orderjson)
			current = current.filter(x => x.id != product.id)
			localStorage.order = JSON.stringify(current)
		} catch (error) {
			this.notif.info(error.message)
			onError()
		}
	}

	addToCart(callback) {
		this.product.provider = this.provider
		let orderjson = localStorage.order
		if (orderjson == null) {
			localStorage.order = JSON.stringify([this.product])
		} else {
			let products = JSON.parse(orderjson)
			let found = false
			for (const item of products) {
				if (item.id == this.product.id) {
					found = true
					item.quantity = this.product.quantity
					item.totalPrice = this.product.totalPrice
					item.comment = this.product.comment
				}
			}
			if (!found) products.push(this.product)
			localStorage.order = JSON.stringify(products)
		}
		callback()
	}

	async getDiscount(order) {
		this.order.discount = 0
		this.order.idPromo = null
		this.order.promoName = ""
		let req = new MyReq(`get discount`)
		req.url = ORDER_DISCOUNT
		req.model = order
		req.success = res => {
			if (res.data) {
				this.order.discount = res.data.discount
				this.order.idPromo = res.data.id
				this.order.promoName = res.data.name
			}
		}
		await this.api.request(req)
	}

	async setOrder(onError) {
		let orderjson = localStorage.order
		if (orderjson == null) {
			this.notif.info("No ha agregado elementos al carrito")
			onError()
			return
		}

		this.order.products = JSON.parse(orderjson).filter(x => x.provider == this.provider)
		if (this.order.products.length == 0) {
			this.notif.info("No ha agregado elementos al carrito")
			onError()
			return
		}
		await this.getDiscount(this.order)
		this.hotelCode = await this.info.hotelCode()
		this.setTotals()
	}

	setTotals() {
		this.order.subtotal = 0
		this.order.total = 0

		for (const item of this.order.products) {
			this.order.total += item.totalPrice
		}

		let total_items = this.order.total
		let additional = 0
		if (this.provider == 4) additional += 20
		this.order.total += additional
		let user = JSON.parse(localStorage.qrh)

		let discount_value = 0
		if (this.order.discount > 0) {
			this.order.total_before_discount = this.order.total
			discount_value = (this.order.discount / 100) * total_items
			this.order.total = total_items - discount_value + additional
			this.order.total_after_discount = this.order.total
		}
		if (this.hotelCode == "PUCAL") {
			this.order.taxes = 0.05 * (this.order.total + discount_value)
			this.order.subtotal = 0.95 * (this.order.total + discount_value)
		} else {
			this.order.taxes = 0.28 * (this.order.total + discount_value)
			this.order.subtotal = 0.72 * (this.order.total + discount_value)
		}

		if (user.tipo == "HOME" && this.provider == 4) {
			//additional += 25 * user.adults
			this.order.total += 25 * user.adults
		}
	}

	async enabled(provider) {
		let response = null
		let req = new MyReq("is cart enabled")
		req.url = PRODUCT_CART_ENABLED
		req.model = { IdProvider: provider }
		req.success = res => {
			response = res
		}
		await this.api.request(req)
		return response
	}

	async saveOrder(callback): Promise<void> {
		this.order.phone = `+${this.order.prefix}${this.order.number}`
		this.order.idProvider = this.provider

		let req = new MyReq("save order")
		req.url = ORDER_SAVE
		req.model = this.order

		req.success = res => {
			this.notif.newOrder(this.provider)
			let model = new OrderMessageModel()
			model.statusCode = "P"
			this.signal.sendOrder(model)
			this.order.products = []
			this.order.comment = ""
			try {
				localStorage.number = this.order.number
				localStorage.prefix = this.order.prefix
				let current = JSON.parse(localStorage.order)
				current = current.filter(x => x.provider != this.provider)
				localStorage.order = JSON.stringify(current)
			} catch (error) {
				localStorage.removeItem("order")
			}
			callback()
		}
		await this.api.request(req)
	}
}
