import { Component, OnInit } from "@angular/core"
import { CartConfigService } from "src/app/services/cart-config.service"
import { InfoService } from "src/app/services/info.service"
import { faPen, faPlus } from "@fortawesome/free-solid-svg-icons"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-admin-cart",
	templateUrl: "./admin-cart.component.html",
	styleUrls: ["./admin-cart.component.css"],
})
export class AdminCartComponent implements OnInit {
	configs = []
	providers = []
	hotels = []
	editIcon = faPen
	addIcon = faPlus
	toadd = {
		hourStart: "",
		hourEnd: "",
		idProvider: 0,
		hotelCode: "",
		active: true,
	}

	clearAdd() {
		this.toadd.hourStart = ""
		this.toadd.hourEnd = ""
		this.toadd.idProvider = 0
		this.toadd.hotelCode = ""
		this.toadd.active = true
	}

	constructor(private cartConfig: CartConfigService, private info: InfoService, private notif: NotificationService) {}

	async loadConfigs(): Promise<void> {
		this.configs = await this.cartConfig.list()
	}

	async ngOnInit(): Promise<void> {
		await this.loadConfigs()
		this.providers = await this.info.loadProviders()
		this.hotels = await this.info.loadHotels()
	}

	async addConfig(): Promise<void> {
		this.toadd.idProvider = +this.toadd.idProvider
		await this.cartConfig.add(this.toadd, () => {
			this.clearAdd()
			this.notif.success("Registrado correctamente.")
			this.loadConfigs()
		})
	}

	async editConfig(index): Promise<void> {
		let toedit = this.configs[index]
		toedit.idProvider = +toedit.idProvider
		await this.cartConfig.edit(toedit, () => {
			this.clearAdd()
			this.notif.success("Actualizado correctamente.")
			this.loadConfigs()
		})
	}
}
