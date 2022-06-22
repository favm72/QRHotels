import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { DIGITAL_LIST } from "src/app/api/endpoints"
import { AnalyticsService } from "src/app/services/analytics.service"
import { ApiService, MyReq } from "src/app/services/api.service"
import { CartService } from "src/app/services/cart.service"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-digital",
	templateUrl: "./digital.component.html",
	styleUrls: ["./digital.component.css"],
})
export class DigitalComponent implements OnInit {
	menu = []

	constructor(
		private router: Router,
		private cart: CartService,
		private api: ApiService,
		private analytics: AnalyticsService,
		private notif: NotificationService
	) {}

	async list() {
		let data = []
		let req = new MyReq("list digital menu")
		req.url = DIGITAL_LIST
		req.success = res => {
			data = res.data
		}
		await this.api.request(req)
		return data
	}

	async ngOnInit(): Promise<void> {
		this.menu = await this.list()
	}

	async goToCart(m): Promise<void> {
		if (m.idProvider != null && m.isCart) {
			this.analytics.log(`cart/${m.idProvider}`)
			this.cart.provider = m.idProvider
			this.cart.viewOnly = m.viewOnly
			// if (provider == 4)
			// 	this.notif.info("El servicio de desayuno a la habitación tiene un costo adicional de s/. 20.00.")
			let verify = await this.cart.enabled(m.idProvider)
			if (verify.data) this.router.navigate(["/cart"])
			else {
				this.notif.info(verify.message)
			}
		}
	}

	linkLaPlaza() {
		this.analytics.log(`LaPlaza`)
		window.open("https://casaandina.contactless.gocloud1.com/cacchi/restaurants/la-plaza/menu", "_blank")
	}

	linkPoolBar() {
		this.analytics.log(`PoolBar`)
		window.open("https://casaandina.contactless.gocloud1.com/cacchi/restaurants/la-plaza/menu", "_blank")
	}

	linkCocinaACasa() {
		this.analytics.log(`CocinaACasa`)
		this.router.navigate(["/master/cocinaacasa"])
	}
}
