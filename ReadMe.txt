Приложение PhotoApplicationBot

1. Написано веб-приложение на net core для просмотра, удаления и поиска фото по тегам. (зависимость таблиц Images и Tags - "отношения многие ко многим"). Поиск по тегам (можно выбрать несколько тегов) показывает результат в виде 1 и более фото.
2. Написан веб-сервис для работы с Telegram.API(Bot), который забирает загруженные фото(с тегами к сообщению) для бота @Ilara90Bot и сохраняет фото и теги в БД(local). Если тег в сообщении под фото отсутствует - сервис генерирует тег в виде даты отправки.