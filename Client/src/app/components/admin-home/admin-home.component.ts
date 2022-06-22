import { Component, OnInit, ViewChild } from "@angular/core"
import { HomeMenuService, HomeMenuType } from "src/app/services/home-menu.service"
import { InfoService } from "src/app/services/info.service"
import { NotificationService } from "src/app/services/notification.service"
import { ModalComponent } from "../utils/modal/modal.component"
import { faEdit } from "@fortawesome/free-solid-svg-icons"
import { InfoPagesService } from "src/app/services/info-pages.service"

const initialForm = () => ({
	description: { value: "", invalid: false },
	title: { value: "", invalid: false },
	hotelCode: { value: "", invalid: false },
	linkUrl: { value: "", invalid: false },
	orderNo: { value: 0, invalid: false },
	imageUrl: { value: "", invalid: false },
	image64: { value: "", invalid: false },
	filename: { value: "", invalid: false },
	active: { value: true, invalid: false },
	isHalf: { value: false, invalid: false },
	link: { value: "", invalid: false },
	idInfoPage: { value: 0, invalid: false },
})

@Component({
	selector: "app-admin-home",
	templateUrl: "./admin-home.component.html",
	styleUrls: ["./admin-home.component.css"],
})
export class AdminHomeComponent implements OnInit {
	editIcon = faEdit
	hotels = []
	infos = []
	currentHotel = ""
	currentItem = null
	items = []
	links = Object.values(HomeMenuType).map(x => ({
		id: x.id,
		label: x.label,
	}))
	form = initialForm()
	@ViewChild("modalForm") modalForm: ModalComponent
	constructor(private home: HomeMenuService, private info: InfoService, private notif: NotificationService, private infoPage: InfoPagesService) {}

	getLabel(link) {
		return HomeMenuType[link].label
	}

	async ngOnInit(): Promise<void> {
		this.hotels = await this.info.loadHotels()
	}

	async loadHomeMenu() {
		if (this.currentHotel == "") this.items = []
		else this.items = await this.home.listAdmin(this.currentHotel)
	}

	async onChangeHotel() {
		this.loadHomeMenu()
		this.infos = await this.infoPage.listHead(this.currentHotel)
	}

	addHomeMenu() {
		this.form = initialForm()
		this.form.hotelCode.value = this.currentHotel
		this.currentItem = null
		this.modalForm.open()
	}

	editHomeMenu(item) {
		this.form = initialForm()
		this.currentItem = item
		this.form.active.value = item.active
		this.form.description.value = item.description
		this.form.orderNo.value = item.orderNo
		this.form.imageUrl.value = item.imageUrl
		this.form.isHalf.value = item.isHalf
		this.form.hotelCode.value = this.currentHotel
		this.form.linkUrl.value = item.linkUrl
		this.form.link.value = item.link ?? ""
		this.form.idInfoPage.value = item.idInfoPage ?? 0
		this.form.title.value = item.title ?? ""
		this.modalForm.open()
	}

	async saveHomeMenu() {
		const model = {
			description: this.form.description.value,
			orderNo: +this.form.orderNo.value,
			image64: this.form.image64.value,
			filename: this.form.filename.value,
			active: this.form.active.value,
			isHalf: this.form.isHalf.value,
			hotelCode: this.form.hotelCode.value,
			linkUrl: this.form.linkUrl.value,
			link: this.form.link.value,
			idInfoPage: this.form.idInfoPage.value,
			title: this.form.title.value,
		}
		let result = false
		if (this.currentItem == null) result = await this.home.create(model)
		else result = await this.home.edit({ ...model, id: this.currentItem.id })

		if (result) {
			this.notif.success(`${this.currentItem == null ? "Registrado" : "Actualizado"} correctamente`)
			this.loadHomeMenu()
			this.modalForm.close()
			this.form = initialForm()
		}
	}
}
